# 开发路线图

> 复选框状态：`[ ]` 已完成 · `[ ]` 待完成
>
> 里程碑版本遵循 [Semantic Versioning](https://semver.org/)。

---

## 阶段 0：基础设施 ✅

_目标：建立可重复构建的解决方案骨架与开发规范。_

- [ ] 解决方案结构（`src/` / `tests/` / `docs/` 分离）
- [ ] 集中式包版本管理（`Directory.Packages.props`）
- [ ] 多框架目标配置（`common.props`：`net8/9/10` + `netstandard2.0/2.1`）
- [ ] 测试公共配置（`common.test.props`：xUnit + FluentAssertions + Moq）
- [ ] 编辑器规范（`.editorconfig`）
- [ ] RFC 文档中文翻译（`docs/rfc/`：RFC 8555、7515 等）
- [ ] 架构文档（`docs/architecture.md`）
- [ ] 开发规范（`docs/development-guide.md`）
- [ ] Copilot 指令文件（`.github/.copilot-instructions.md`）

---

## 阶段 1：协议模型层 ✅ `v0.1`

_目标：定义与 ACME 服务器通信所需的所有数据模型与异常体系。（现归属 `Acme.Protocol.Shared`）_

- [ ] HTTP 请求模型（`AccountCreateModel`、`OrderCreateModel`、`OrderFinalizeModel` 等）
- [ ] HTTP 响应模型（`DirectoryModel`、`AccountModel`、`OrderModel`、`AuthorizationModel` 等）
- [ ] ACME 枚举（`ChallengeStatus`、`ChallengeType`、`ContactType`、`CertificateRevokeReasonCode` 等）
- [ ] ACME 异常体系（`AcmeException` 基类及各派生异常）
- [ ] Renewal Info 模型（`RenewalInfoModel`，RFC 8739）
- [ ] 订单列表模型（`OrderListModel`，RFC 9447）

---

## 阶段 2：核心加密层 ✅ `v0.2`

_目标：实现 JWK/JWS 签名及 X.509/CSR 工具，为客户端提供加密原语。（现归属 `Acme.Protocol.Shared`）_

- [ ] JWK 接口（`IJsonWebKey`）与抽象基类（`JsonWebKey`）
- [ ] JWK 具体实现（`RsaJsonWebKey`、`EcJsonWebKey`、`OctJsonWebKey`）
- [ ] JWS 签名器接口（`IJwkSigner`）与实现（`DefaultJwkSigner`）
- [ ] 算法常量（`JsonWebKeyAlgorithms`）
- [ ] X.509 工具（`CertUtils`、`PemFormatter`、`CertificateInfo`、`PemHeader`）
- [ ] CSR 生成工具（`CsrUtils`、`KeyUtils`）
- [ ] BouncyCastle 扩展（`HmacSigner`、`ISignerExtensions`、`IDigestExtensions`）
- [ ] Base64Url JSON 转换器（`ByteArrayBase64UrlStringJsonConverter`）
- [ ] JSON 序列化封装（`SystemTextJsonSerializer`、`IJsonSerializer`）
- [ ] 目录 URL 规范化工具（`DirectoryUrlNormalizer`）

---

## 阶段 3：客户端协议实现 🚧 `v0.3`

_目标：实现完整的 ACME 客户端，覆盖 RFC 8555 定义的全流程 API。（`Acme.Protocol.Client`，位于 `src/Shared/Acme.Protocol.Client`）_

**协议客户端核心实现：**

- [ ] `AcmeHttpClient` / `AcmeHttpClientFactory`（HTTP 通信层）
- [ ] `IAcmeProtocolClientFactory` / `AcmeProtocolClientFactory`（工厂模式）
- [ ] `IAcmeAccountContext` / `AcmeAccountContext`（账户上下文管理）
- [ ] `IAcmeProtocolClient` / `AcmeProtocolClient` 实现（完整 ACME 流程 11 个核心方法）
  - [ ] `GetDirectoryAsync` - 获取 ACME 服务器目录
  - [ ] `ConsumeNonceAsync` - 获取一次性随机数（RFC 8555 §7.2）
  - [ ] `CreateAccountAsync` - 创建新账户（RFC 8555 §7.3）
  - [ ] `CheckAccountAsync` - 检查现有账户（RFC 8555 §7.3.1）
  - [ ] `KeyChangeAsync` - 账户密钥更换（RFC 8555 §7.3.6）
  - [ ] `DeactivateAccountAsync` - 账户停用（RFC 8555 §7.3.7）
  - [ ] `UpdateAccountAsync` - 更新账户信息（RFC 8555 §7.3.2）
  - [ ] `CreateOrderAsync` - 申请证书订单（RFC 8555 §7.4）
  - [ ] `GetOrderDetailsAsync` - 获取订单详情（RFC 8555 §7.4）
  - [ ] `ListOrdersAsync` - 订单列表查询（RFC 8555 §7.1.2.1）
  - [ ] `GetAuthorizationDetailAsync` - 获取授权详情（RFC 8555 §7.5）
  - [ ] `AnswerChallengeAsync` - 回应挑战（RFC 8555 §7.5.1）
  - [ ] `FinalizeOrderAsync` - 最终化订单（RFC 8555 §7.4）
  - [ ] `CollectOrderCertificateAsync` - 下载签发证书（RFC 8555 §7.4.2，支持 PEM/PFX 多格式）
  - [ ] `RevokeCertificateAsync` - 证书吊销（RFC 8555 §7.6）
  - [ ] `GetRenewalInfoAsync` - 获取续订建议信息（RFC 8739）
  - [ ] `UpdateRenewalInfoAsync` - 更新续订信息（RFC 8739）
- [ ] `AcmeProtocolClientOptions` 完善（超时、Polly 重试策略、BadNonce 自动重试、Nonce 缓存容量、User-Agent 等可配置项）

---

## 阶段 4：测试覆盖 🚧 `v0.3`

_目标：关键模块单元测试覆盖率 ≥ 80%，集成测试验证真实 ACME 流程。_

- [ ] `Acme.Base.Tests`：枚举扩展测试（`EnumExtensionsTests`）
- [ ] `Acme.Base.Tests`：字符串扩展测试（`StringExtensionsTests`）
- [ ] `Acme.Base.Tests`：JSON 序列化测试（`SystemTextJsonSerializerTests`）
- [ ] `Acme.Protocol.Client.Tests`：集成测试框架（Let's Encrypt Staging）
- [ ] `Acme.Protocol.Shared.Tests`：JWK 单元测试（RSA / EC / Oct 密钥生成、Thumbprint）
- [ ] `Acme.Protocol.Shared.Tests`：JWS 签名器单元测试（签名 / 验签 / 算法覆盖）
- [ ] `Acme.Protocol.Shared.Tests`：X.509 / CSR 工具单元测试
- [ ] `Acme.Protocol.Client.Tests`：协议客户端单元测试（Moq HTTP 层）
- [ ] `Acme.Protocol.Shared.Tests`：异常体系测试
- [ ] CI 覆盖率门控（coverlet + 阈值检查，目标 ≥ 80%）

---

## 阶段 5：共享基础库 + 客户端核心 📋 `v0.4`

_目标：完善两个共享库，以 DDD 实现语义丰富的证书管理核心，插件提供 DNS 提供商支持。_

**`Acme.Core.Shared`（`src/Shared/Acme.Core.Shared`）：**

- [ ] `IAggregateRoot`、`IEntity<TKey>`、`IValueObject` 基础接口
- [ ] `IDomainEvent` 接口 + `IDomainEventHandler<TEvent>` + `IDomainEventDispatcher` 接口
- [ ] `IRepository<T, TKey>` 泛型仓储接口 + `IUnitOfWork`
- [ ] `AuditedEntity<TKey>`（CreatedAt / UpdatedAt 等审计字段基类）
- [ ] 通用工具扩展（`StringExtensions`、`EnumExtensions`、`HexConverter` 等非协议相关工具）

**`Acme.Protocol.Shared`（扩展，`src/Shared/Acme.Protocol.Shared`）：**

- [ ] `IJwsVerifier` 接口 + `DefaultJwsVerifier` 实现（JWS 验签能力，服务端复用）
- [ ] `JwsVerificationException`（验签失败专用异常，`AcmeException` 派生类）

**`Acme.Client.Core`（`src/Client/Acme.Client.Core`）：**

- [ ] `Certificate` 聚合根（`ChallengeMode`、`AliasDomain?`、`DnsProviderType?`、`AccountId` 等字段；继承 `Acme.Core.Shared` 的聚合根基类）
- [ ] `Account` 实体（多账户支持，`AccountId` 主键；继承 `Acme.Core.Shared` 的实体基类）
- [ ] `ChallengeMode` 枚举（`Http01Standalone`/`Http01HttpForwarding`/`DnsApiProvider`/`DnsAlias`/`DnsManual`）
- [ ] `CertificateStatus` 枚举（`Pending`/`Active`/`Expiring`/`Expired`/`Revoked`）
- [ ] `IDomainEventHandler<TEvent>` 接口 + `IDomainEventDispatcher` 接口（由 `Acme.Core.Shared` 提供，此处直接引用）
- [ ] `ICertificateRepository`/`IAccountRepository` 仓储接口（继承 `Acme.Core.Shared` 的 `IRepository<T>`）
- [ ] `IDnsProvider` 挑战插件接口
- [ ] `IChallengeResponder`/`IHttp01ChallengeResponder`/`IDns01ChallengeResponder` 接口

**`Acme.Client.Application`（`src/Client/Acme.Client.Application`）：**

- [ ] `RequestCertificateCommand`/`Handler`（下单→挑战→签发，返回 `CertificateDto`）
- [ ] `RenewCertificateCommand`/`Handler`
- [ ] `RevokeCertificateCommand`/`Handler`
- [ ] `ExportCertificateCommand`/`Handler`（`X509Certificate2.Export` 输出 PFX）
- [ ] `ListCertificatesQuery`/`Handler`
- [ ] `GetCertificateQuery`/`Handler`
- [ ] `RegisterAccountCommand`/`Handler`
- [ ] `ICertificateService` 外层服务接口（供 Host HTTP 控制器 DI 注入）

**`Acme.Client.Infrastructure`（`src/Client/Acme.Client.Infrastructure`）：**

- [ ] `InProcessDomainEventDispatcher`（`IServiceProvider` 解析 Handler 同步分发，实现 `Acme.Core.Shared` 的 `IDomainEventDispatcher`）
- [ ] `FileCertificateRepository`（JSON 元数据 + PEM，Linux/macOS `chmod 600`）
- [ ] `FileAccountRepository`
- [ ] `Dns01ChallengeResponder`（查询权威 NS，10 s 间隔，10 min 超时抛 `DnsPropagationTimeoutException`）
- [ ] `Http01StandaloneChallengeResponder`（内置 Kestrel）
- [ ] `Http01HttpForwardingChallengeResponder`（写临时文件/外联指引）
- [ ] `AcmeClientOptions`（`ExpiringThresholdDays` = 30、`DnsPropagationTimeoutSeconds`）
- [ ] Core DI 扩展方法（`AddAcmeClientCore`）

**`Acme.Client.Dns.Provider.Aliyun`（`src/DnsProviders/Acme.Client.Dns.Provider.Aliyun`）：**

**`Acme.Server.Dns.Provider.Aliyun`（`src/Server/DnsProviders/Acme.Server.Dns.Provider.Aliyun`）：**

- [ ] `AliyunDnsProvider`（`IDnsProvider` 实现，`alidns.aliyuncs.com`、HMAC-SHA1 v1）
- [ ] `AliyunDomainParser`（主域名解析）
- [ ] `AliyunDnsOptions`（`AccessKeyId`/`AccessKeySecret`）
- [ ] DI 注册扩展（`AddAliyunDnsProvider`）
- [ ] 单元测试

---

## 阶段 6：HTTP/2 API 契约 📋 `v0.5`

_目标：定义 CLI 与 Web 前端共用的 HTTP/2 RESTful API，由 Host 提供。_

- [ ] 创建 `Acme.Client.Host` 项目（`src/Client/`）
- [ ] 设计 HTTP RESTful API 端点（`/api/certificates`、`/api/accounts` 等）
- [ ] 定义 DTO 和请求/响应模型
- [ ] 实现 API 文档（OpenAPI/Swagger）
- [ ] 配置 API Key 认证中间件（检查 `x-api-key` 请求头）

---

## 阶段 7：Web UI + 后端宿主 📋 `v0.5`

_目标：`Acme.Client.Web` 提供 Vue 前端 SPA；`Acme.Client.Host` 作为 ASP.NET Core 宿主提供 HTTP/2 RESTful API。_

**`Acme.Client.Web`（`src/Client/Acme.Client.Web`）：**

- [ ] 创建 Vue 3 + TypeScript 项目（Vite）
- [ ] 实现证书管理页面（列表、申请、续期、导出、吊销）
- [ ] 实现账户管理页面
- [ ] Pinia 状态管理（证书、账户、认证状态）
- [ ] HTTP 客户端和拦截器（处理 API Key、错误）
- [ ] 前后端分离部署配置

**`Acme.Client.Host`（`src/Client/Acme.Client.Host`）：**

- [ ] HTTP/2 RESTful 控制器实现（证书、账户、进度）
- [ ] 调用 Application Handler 并返回 JSON 响应
- [ ] `CertificateRenewalBackgroundService`（`IHostedService`，后台续期）
- [ ] DI 配置和 `appsettings.json` 框架
- [ ] CORS 配置（允许前端跨域请求）
- [ ] 集成测试

---

## 阶段 8：CLI 客户端 📋 `v0.5`

_目标：实现 Spectre.Console.Cli 命令行工具，通过 HTTP/2 RESTful API 与 Host 通信。_

- [ ] 创建 `Acme.Client.Cli` 项目（`src/Client/`）
- [ ] HTTP 客户端工厂（读取配置、注入 API Key）
- [ ] `cert request` 命令（`--domain`、`--challenge`、`--alias-domain`）+ 进度反馈
- [ ] `cert renew` 命令 + 进度反馈
- [ ] `cert list`、`cert export`、`cert revoke` 命令
- [ ] `account register`、`account list` 命令
- [ ] 错误处理与 Spectre 富文本输出
- [ ] 自包含单文件可执行发布配置（`PublishSingleFile`）
- [ ] CI 构建集成

---

## 阶段 9：服务端实现 📋 `v0.8`

_目标：实现可嵌入 ASP.NET Core 的 ACME 服务端，支持自建 CA 场景，覆盖 RFC 8555 完整端点。_

**`Acme.Protocol.Shared`（扩展，阶段 9 前置，已在阶段 5 完成）：**

- [ ] `IJwsVerifier` 接口 + `DefaultJwsVerifier` 实现（JWS 验签，校验 `nonce`/`url`/`alg` 字段）
- [ ] `JwsVerificationException`（验签失败专用异常，`AcmeException` 派生类）

**`Acme.Server.Core`（`src/Server/Acme.Server.Core`）：**

- [ ] `Account` 聚合根（kid、status、contacts、orders 列表、EAB binding 状态；继承 `Acme.Core.Shared` 聚合根基类）
- [ ] `Order` 聚合根（status、identifiers、authorizations、certificate URL、expires、notBefore、notAfter）
- [ ] `Authorization` 实体（identifier、status、challenges、wildcard 标志、expires）
- [ ] `Challenge` 实体（type、status、token、validated、error，支持 Http01/Dns01/TlsAlpn01）
- [ ] `Certificate` 实体（serial、pemChain、status、issuedAt、expiresAt）
- [ ] `ICAProvider` 接口（`IssueCertificateAsync` / `RevokeCertificateAsync`）
- [ ] `IServerChallengeValidator` 接口体系（Http01 / Dns01 / TlsAlpn01 三子接口）
- [ ] `INonceRepository` 接口（`GenerateAsync` / `ConsumeAsync`，防重放）
- [ ] `IDistributedNonceRepository` 扩展接口（分布式场景预留）
- [ ] `IExternalAccountBindingService` 接口（RFC 8555 §7.3.4 EAB 验证）
- [ ] 服务端仓储接口：`IServerAccountRepository` / `IOrderRepository` / `IAuthorizationRepository` / `IServerCertificateRepository`（继承 `Acme.Core.Shared` 的 `IRepository<T>`）

**`Acme.Server.Infrastructure`（`src/Server/Acme.Server.Infrastructure`）：**

- [ ] `BouncyCastleCAProvider`（`ICAProvider` 实现；优先读取外部 PEM，否则首次启动自动生成自签根 CA）
- [ ] `AcmeServerDbContext`（EFCore + PostgreSQL，`Npgsql.EntityFrameworkCore.PostgreSQL`）
- [ ] `EfCoreAccountRepository` / `EfCoreOrderRepository` / `EfCoreAuthorizationRepository` / `EfCoreCertificateRepository`（EFCore 仓储实现）
- [ ] 数据库迁移脚本（`Migrations/`）
- [ ] `InMemoryNonceRepository`（`ConcurrentDictionary` + TTL 滑动过期，Nonce 不持久化）
- [ ] `Http01ChallengeValidator`（向目标 `/.well-known/acme-challenge/{token}` 发 GET 请求验证，RFC 8555 §8.3）
- [ ] `Dns01ChallengeValidator`（查询 `_acme-challenge.{domain}` TXT 记录，RFC 8555 §8.4）
- [ ] `TlsAlpn01ChallengeValidator`（TLS 握手验证 `acme-tls/1` ALPN 证书扩展，RFC 8737）
- [ ] DI 扩展（`AddAcmeServerInfrastructure`）

**`Acme.Server.Protocol`（`src/Server/Acme.Server.Protocol`）：**

- [ ] `DirectoryEndpoints`（`GET /directory`，RFC 8555 §7.1.1）
- [ ] `NonceEndpoints`（`HEAD/GET /acme/new-nonce`，RFC 8555 §7.2）
- [ ] `AccountEndpoints`（`POST /acme/new-account`、`POST /acme/acct/{id}`；含 key-change、deactivate、EAB，RFC 8555 §7.3、§7.3.4）
- [ ] `OrderEndpoints`（`POST /acme/new-order`、`GET /acme/order/{id}`、`GET /acme/orders`、`POST /acme/order/{id}/finalize`，RFC 8555 §7.4、RFC 9447）
- [ ] `AuthorizationEndpoints`（`GET /acme/authz/{id}`，RFC 8555 §7.5）
- [ ] `ChallengeEndpoints`（`GET/POST /acme/chall/{id}`，RFC 8555 §7.5.1）
- [ ] `CertificateEndpoints`（`GET /acme/cert/{id}`、`POST /acme/revoke-cert`，RFC 8555 §7.4.2、§7.6）
- [ ] `JwsVerificationFilter`（`IEndpointFilter`；Nonce 消费 + `IJwsVerifier` 验签 + URL/alg 校验）
- [ ] `AcmeEndpointRouteBuilderExtensions`（`MapAcmeServer()` 注册所有端点并附加 Filter）
- [ ] `AcmeServerProtocolOptions`（`RoutePrefix` 默认 `/`，Nonce 响应头名称等）

**`Acme.Server.Host`（`src/Server/Acme.Server.Host`）：**

- [ ] `AcmeServerServiceCollectionExtensions`（`AddAcmeServer(options)` 注册所有服务）
- [ ] `AcmeServerOptions`（CA 路径/自动生成开关、EAB 开关、路由前缀、挑战轮询间隔等）
- [ ] `ChallengeValidationBackgroundService`（`BackgroundService`；轮询 pending Challenge，按 type 分发到 `IServerChallengeValidator`，写回聚合根状态机）

**测试（`tests/Server/`）：**

- [ ] `Acme.Server.Core.Tests`：聚合根状态机单元测试（Account / Order / Authorization / Challenge 状态转换）
- [ ] `Acme.Server.Infrastructure.Tests`：`BouncyCastleCAProvider` 签发与吊销测试；`InMemoryNonceRepository` 防重放测试；EFCore 仓储集成测试（Testcontainers PostgreSQL）；三种 ChallengeValidator 单元测试
- [ ] `Acme.Server.Protocol.Tests`：`JwsVerificationFilter` 单元测试（Moq `INonceRepository` + `IJwsVerifier`）；各端点 HTTP 层集成测试（`WebApplicationFactory`）
- [ ] `Acme.Server.Integration.Tests`：`Acme.Client.Protocol` ↔ `Acme.Server.Host` 全流程端到端测试

---

## 阶段 10：发布与运维 📋 `v1.0`

_目标：完成 NuGet 发布准备，建立自动化 CI/CD 流水线。_

- [ ] NuGet 元数据配置（`PackageId`、`PackageDescription`、`Authors`、`RepositoryUrl`、`PackageLicense`）
- [ ] 发布到 nuget.org：`Acme.Protocol.Client`
- [ ] 发布到 nuget.org：`Acme.Protocol.Shared`
- [ ] 发布到 nuget.org：`Acme.Core.Shared`
- [ ] GitHub Actions：`build.yml`（PR 触发；dotnet build + test + format check）
- [ ] GitHub Actions：`release.yml`（tag 触发；dotnet pack + nuget push）
- [ ] CI 覆盖率徽章（Codecov 或 coveralls）
- [ ] CHANGELOG 维护（遵循 Keep a Changelog + semver）
- [ ] API 文档生成（DocFX 或 xmldoc → GitHub Pages）
- [ ] README 完善（快速上手示例、徽章、许可证说明）
