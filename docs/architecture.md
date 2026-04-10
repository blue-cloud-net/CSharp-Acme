# 架构文档

## 项目定位

CSharp-Acme 是 [RFC 8555](https://datatracker.ietf.org/doc/html/rfc8555)（ACME 协议）的 C# 完整实现，  
提供**客户端协议库、客户端应用与服务端实现**，支持 TLS/SSL 证书的自动化申请、续期、吊销，  
多目标框架支持（`netstandard2.0/2.1` + `net8/9/10`）。

共享层分为两个库：

- **`Acme.Protocol.Shared`**（`src/Shared/`）：ACME 协议 HTTP 模型、JWK/JWS 加解密、X.509/CSR 工具等与 ACME 协议及加密直接相关的内容，由客户端协议层（`Acme.Protocol.Client`）引用。
- **`Acme.Core.Shared`**（`src/Shared/`）：客户端与服务端共同依赖的内容，包括 DDD 基础接口/基类、通用值对象、工具类扩展等，由 `Acme.Client.Core` 和 `Acme.Server.Core` 同时引用。

---

## 解决方案分层

### 客户端分层

```
┌─────────────────────────────────────────┐  ┌──────────────────────────────────────────┐
│      Acme.Client.Web                    │  │      Acme.Client.Cli                     │
│  Vue 3 + TypeScript (SPA)               │  │  Spectre.Console.Cli 命令行工具           │
│  Vite · TypeScript · pinia              ├──┤  调用 Host HTTP/2 API                    │
│  HTTP/2 客户端                           │  │  自包含单文件可执行                        │
└──────────────┬──────────────────────────┘  └──────────────┬───────────────────────────┘
               │ 调用 HTTP/2 API                             │ 调用 HTTP/2 API
               └─────────────────┬──────────────────────────┘
                                 │
                ┌────────────────▼───────────────────┐
                │      Acme.Client.Host             │
                │  ASP.NET Core RESTful API         │
                │  BackgroundService · API Key 认证 │
                └────────────────┬───────────────────┘
                                 │ 依赖
┌────────────────────────────────▼──────────────────────────────┐
│             Acme.Client.Application                           │
│  CQRS Handler · Command/Query/DTO · ICertificateService       │
└────────────────────────────────┬──────────────────────────────┘
                                 │ 依赖
┌────────────────────────────────▼──────────────────────────────┐
│                Acme.Client.Core                               │
│    聚合根/实体 · 领域事件 · 仓储接口 · IDnsProvider              │
└────────────────────────────────┬──────────────────────────────┘
                                 │ 依赖（实现）
┌────────────────────────────────▼──────────────────────────────┐
│            Acme.Client.Infrastructure                         │
│     File 仓储 · 挑战响应器 · DI 扩展                             │
└────────────────────────────────┬──────────────────────────────┘
                                 │ 依赖
┌────────────────────────────────▼──────────────────────────────┐
│                Acme.Protocol.Client                           │
│         IAcmeProtocolClient / AcmeHttpClient                  │
│        客户端协议层（netstandard2.0/2.1 + net8/9/10）           │
└────────────────────────────────┬──────────────────────────────┘
                                 │ 依赖
┌────────────────────────────────▼──────────────────────────────┐
│                Acme.Protocol.Shared                           │
│  HTTP 模型 / JWK / JWS / X.509 / CSR / 枚举 / 异常体系        │
│       协议共享层（netstandard2.0/2.1 + net8/9/10）            │
└─────────────────────────────────────────────────────────────┘
```

---

### 服务端分层

```
┌────────────────────────────────────────────────────┐
│                  Acme.Server.Host                  │
│  AddAcmeServer() · MapAcmeServer()                 │
│  ChallengeValidationBackgroundService              │
└───────────────────────────┬────────────────────────┘
                            │ 依赖
┌───────────────────────────▼────────────────────────┐
│                Acme.Server.Protocol                │
│      Minimal API 端点 · JwsVerificationFilter      │
│       端点 Handler 直接编排领域服务（兼任应用层）      │
│               服务端协议层（net8/9/10）              │
└───────────────────────────┬────────────────────────┘
                            │ 依赖
┌───────────────────────────▼────────────────────────┐
│                  Acme.Server.Core                  │
│  聚合根/实体 · ICAProvider · INonceRepository       │
│  IServerChallengeValidator · EAB · 仓储接口         │
└───────────────────────────┬────────────────────────┘
                            │ 依赖（实现）
┌───────────────────────────▼────────────────────────┐
│            Acme.Server.Infrastructure              │
│  BouncyCastleCAProvider · InMemoryNonceRepository  │
│  Http01 / Dns01 / TlsAlpn01 ChallengeValidator     │
│  InMemory 仓储（Account / Order / Authz / Cert）   │
└───────────────────────────┬────────────────────────┘
                            │ 依赖
┌───────────────────────────▼────────────────────────┐
│                 Acme.Protocol.Shared               │
│   HTTP 模型 / JWK / JWS / X.509 / CSR / 枚举 / 异常  │
│       协议共享层（netstandard2.0/2.1 + net8/9/10）   │
└────────────────────────────────────────────────────┘
```

---

## 各层职责

| 项目                              | 命名空间前缀                   | 职责                                                                                                                                                                                 | 关键类型                                                                                                                                                                                                                                                                             |
| --------------------------------- | ------------------------------ | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| `Acme.Protocol.Shared`            | `Acme.*`                       | ACME 协议 HTTP 请求/响应模型；枚举；异常体系；JWK/JWS 密钥、签名与验签；X.509/CSR 工具；哈希/PEM 格式化；JSON/字符串/枚举辅助（协议及加密相关内容的唯一共享库）                      | `DirectoryModel`、`OrderModel`、`AcmeException`、`IJsonWebKey`、`IJwkSigner`、`IJwsVerifier`、`CertUtils`、`CsrUtils`、`PemFormatter`、`HmacSigner`、`SystemTextJsonSerializer`、`EnumExtensions`                                                                                    |
| `Acme.Core.Shared`                | `Acme.*`                       | 客户端与服务端共同依赖的 DDD 基础接口/基类；通用值对象；工具类扩展（非 ACME 协议内容）                                                                                               | `IAggregateRoot`、`IEntity`、`IDomainEvent`、`IDomainEventHandler<TEvent>`、`IDomainEventDispatcher`、`IRepository<T>`、`AuditedEntity`                                                                                                                                              |
| `Acme.Protocol.Client`            | `Acme.Protocol.Client.*`       | ACME 客户端协议实现；账户上下文管理；DI 集成（位于 `src/Shared/`，供客户端各层引用）                                                                                                 | `IAcmeProtocolClient`、`AcmeProtocolClient`、`IAcmeAccountContext`、`IAcmeProtocolClientFactory`                                                                                                                                                                                     |
| `Acme.Client.Core`                | `Acme.Client.Core.*`           | 客户端 DDD 领域核心；聚合根/实体/值对象；仓储接口；`IDnsProvider`（引用 `Acme.Core.Shared` 获取 DDD 基础接口）                                                                       | `Certificate`、`Account`、`ICertificateRepository`、`IAccountRepository`、`IDnsProvider`                                                                                                                                                                                             |
| `Acme.Client.Application`         | `Acme.Client.Application.*`    | CQRS Handler；Command/Query/DTO；`ICertificateService` 服务接口                                                                                                                      | `RequestCertificateHandler`、`CertificateDto`、`ICertificateService`                                                                                                                                                                                                                 |
| `Acme.Client.Infrastructure`      | `Acme.Client.Infrastructure.*` | 仓储实现；挑战响应器；DI 扩展                                                                                                                                                        | `FileCertificateRepository`、`Dns01ChallengeResponder`、`Http01StandaloneChallengeResponder`                                                                                                                                                                                         |
| `Acme.Client.Host`                | `Acme.Client.Host.*`           | ASP.NET Core 宿主；HTTP/2 RESTful API 端点；BackgroundService；API Key 认证；供 CLI 与 Web 前端调用                                                                                  | RESTful Controllers、`CertificateRenewalBackgroundService`、HttpClient 工厂                                                                                                                                                                                                          |
| `Acme.Client.Cli`                 | `Acme.Client.Cli.*`            | Spectre.Console.Cli 命令树；HTTP/2 客户端；自包含单文件可执行                                                                                                                        | `AsyncCommand<TSettings>` 实现、HTTP 客户端工厂                                                                                                                                                                                                                                      |
| `Acme.Client.Web`                 | —                              | Vue 3 + TypeScript SPA；HTTP/2 客户端；前后端分离；独立部署                                                                                                                          | Vue 组件、TypeScript / Vite 打包、Pinia 状态管理、HTTP 拦截器                                                                                                                                                                                                                        |
| `Acme.Client.Dns.Provider.Aliyun` | `Acme.Client.Dns.Provider.*`   | 阿里云 DNS 插件；`IDnsProvider` 实现；HMAC-SHA1 v1 签名                                                                                                                              | `AliyunDnsProvider`、`AliyunDomainParser`、`AliyunDnsOptions`                                                                                                                                                                                                                        |
| `Acme.Server.Protocol`            | `Acme.Server.*`                | ACME Minimal API 端点；JWS 请求验签过滤器；端点 Handler 直接编排领域服务（兼任应用层）                                                                                               | `AcmeEndpointRouteBuilderExtensions`、`DirectoryEndpoints`、`NonceEndpoints`、`AccountEndpoints`、`OrderEndpoints`、`AuthorizationEndpoints`、`ChallengeEndpoints`、`CertificateEndpoints`、`JwsVerificationFilter`                                                                  |
| `Acme.Server.Core`                | `Acme.Server.Core.*`           | 服务端 DDD 领域核心；聚合根/实体；`ICAProvider`；`INonceRepository`；`IServerChallengeValidator`；`IExternalAccountBindingService`（EAB）；服务端仓储接口（引用 `Acme.Core.Shared`） | `Account`、`Order`、`Authorization`、`Challenge`、`Certificate`、`ICAProvider`、`INonceRepository`、`IDistributedNonceRepository`、`IExternalAccountBindingService`、`IServerAccountRepository`、`IOrderRepository`、`IAuthorizationRepository`、`IServerCertificateRepository`      |
| `Acme.Server.Infrastructure`      | `Acme.Server.Infrastructure.*` | CA 实现；EFCore + PostgreSQL 仓储实现；Nonce 内存存储；三种挑战验证器；DI 扩展                                                                                                       | `BouncyCastleCAProvider`、`AcmeServerDbContext`、`EfCoreAccountRepository`、`EfCoreOrderRepository`、`EfCoreAuthorizationRepository`、`EfCoreCertificateRepository`、`InMemoryNonceRepository`、`Http01ChallengeValidator`、`Dns01ChallengeValidator`、`TlsAlpn01ChallengeValidator` |
| `Acme.Server.Host`                | `Acme.Server.Host.*`           | ASP.NET Core 宿主集成；`AddAcmeServer` DI 入口；挑战后台验证服务                                                                                                                     | `AcmeServerServiceCollectionExtensions`、`AcmeServerOptions`、`ChallengeValidationBackgroundService`                                                                                                                                                                                 |

---

## 关键接口

### `IJsonWebKey`（Acme.Protocol.Shared）

表示 JSON Web Key（JWK），提供密钥类型、密钥 ID 及 JWK Thumbprint 计算能力。  
实现类：`RsaJsonWebKey`、`EcJsonWebKey`、`OctJsonWebKey`。

### `IJwkSigner`（Acme.Protocol.Shared）

封装 JWS 签名操作，支持 RS256/RS384/RS512、ES256/ES384/ES512 等算法。  
实现类：`DefaultJwkSigner`。

### `IJwsVerifier`（Acme.Protocol.Shared）

封装 JWS 验签操作，与 `IJwkSigner` 对称，供服务端 `JwsVerificationFilter` 调用。  
方法：`VerifyAsync(jwsFlattenedJson, ct)` 返回解析后的 JWS Payload；验签失败抛 `JwsVerificationException`（`AcmeException` 派生类）。  
实现类：`DefaultJwsVerifier`。

### `IAcmeAccountContext`（Acme.Protocol.Client）

维护与 ACME 服务器交互的账户状态：服务器目录 URL、当前账户、签名器实例及 Nonce 缓存。

### `IAcmeProtocolClient`（Acme.Protocol.Client）

ACME 协议完整操作集，覆盖 RFC 8555 定义的全流程：

| 方法                           | 对应 RFC 章节  |
| ------------------------------ | -------------- |
| `GetDirectoryAsync`            | §7.1.1         |
| `ConsumeNonceAsync`            | §7.2           |
| `CreateAccountAsync`           | §7.3           |
| `CheckAccountAsync`            | §7.3.1、§7.3.3 |
| `UpdateAccountAsync`           | §7.3.2         |
| `KeyChangeAsync`               | §7.3.6         |
| `DeactivateAccountAsync`       | §7.3.7         |
| `ListOrdersAsync`              | §7.1.2.1       |
| `CreateOrderAsync`             | §7.4、§7.1.3   |
| `GetOrderDetailsAsync`         | §7.4           |
| `GetAuthorizationDetailAsync`  | §7.5           |
| `AnswerChallengeAsync`         | §7.5.1         |
| `FinalizeOrderAsync`           | §7.4           |
| `CollectOrderCertificateAsync` | §7.4.2         |
| `RevokeCertificateAsync`       | §7.6           |
| `GetRenewalInfoAsync`          | RFC 8739       |
| `UpdateRenewalInfoAsync`       | RFC 8739       |

### `IAcmeProtocolClientFactory`（Acme.Protocol.Client）

工厂接口，用于创建绑定特定账户上下文的 `IAcmeProtocolClient` 实例，便于 DI 注入和多账户场景。

### `ICertificateService`（Acme.Client.Application）

应用层统一入口，由 `Acme.Client.Host` 的 HTTP 控制器调用（DI 注入）。封装证书请求、续期、吊销、导出及查询操作，内部转发至对应 CQRS Handler。CLI 和 Web 前端通过 HTTP API 间接调用。

### `ICertificateRepository` / `IAccountRepository`（Acme.Client.Core）

仓储接口，定义证书与账户的持久化契约。默认实现 `FileCertificateRepository` / `FileAccountRepository` 位于 `Acme.Client.Infrastructure`，以 JSON 元数据 + PEM 文件存储，在 Linux/macOS 上对私钥文件执行 `chmod 600`。

### `IChallengeResponder` / `IHttp01ChallengeResponder` / `IDns01ChallengeResponder`（Acme.Client.Core / Acme.Client.Infrastructure）

挑战响应器接口体系，按挑战类型分离。`Dns01ChallengeResponder` 查询权威 NS 验证 DNS 传播（10 s 间隔，10 min 超时，超时抛 `DnsPropagationTimeoutException`）。支持的 `ChallengeMode`：

| 模式                   | 说明                                  |
| ---------------------- | ------------------------------------- |
| `Http01Standalone`     | 内置 Kestrel 监听 HTTP-01 验证路径    |
| `Http01HttpForwarding` | 写临时文件 / 外联指引（反代场景）     |
| `DnsApiProvider`       | 调用 `IDnsProvider` 自动增删 TXT 记录 |
| `DnsAlias`             | CNAME 委派至别名域名，再调用 DNS API  |
| `DnsManual`            | 输出 TXT 记录值，等待人工确认后继续   |

通配符域名（`*.example.com`）在 Application 层强制要求 DNS 模式，否则抛 `InvalidChallengeModeException`。

### `IDnsProvider`（Acme.Client.Core）

DNS 提供商插件接口，定义于领域层。

- **客户端实现** `AliyunDnsProvider` 位于 `Acme.Client.Dns.Provider.Aliyun`（`src/DnsProviders/`，供 `Dns01ChallengeResponder` 调用）
- **服务端实现** `AliyunDnsProvider` 位于 `Acme.Server.Dns.Provider.Aliyun`（`src/Server/DnsProviders/`，供 `Dns01ChallengeValidator` 调用）

两者均基于阿里云 DNS API（`alidns.aliyuncs.com`，HMAC-SHA1 v1 Query String 签名，无 SDK 依赖）。

### `IDomainEventDispatcher`（Acme.Client.Core）

领域事件调度器接口。默认实现 `InProcessDomainEventDispatcher`，通过 `IServiceProvider` 解析所有 `IDomainEventHandler<TEvent>` 并同步分发。

---

## 服务端关键接口

### `ICAProvider`（Acme.Server.Core）

CA 插件接口，解耦证书签发实现，支持自建根 CA 或接入外部 PKI 系统。  
方法：`IssueCertificateAsync(csrInfo, ct)` → `IssuedCertificate`；`RevokeCertificateAsync(serialNumber, reason, ct)`。  
默认实现 `BouncyCastleCAProvider`（`Acme.Server.Infrastructure`）：优先读取 `AcmeServerOptions.CaPemPath` + `CaKeyPath` 外部根证书；否则首次启动自动生成自签根 CA 并持久化。

### `IServerChallengeValidator`（Acme.Server.Core / Acme.Server.Infrastructure）

服务端挑战验证器接口体系，按挑战类型分离为三个子接口：

| 实现类                        | 挑战类型    | 验证方式                                                            | 关联 RFC      |
| ----------------------------- | ----------- | ------------------------------------------------------------------- | ------------- |
| `Http01ChallengeValidator`    | HTTP-01     | 向 `http://{domain}/.well-known/acme-challenge/{token}` 发 GET 请求 | RFC 8555 §8.3 |
| `Dns01ChallengeValidator`     | DNS-01      | 查询 `_acme-challenge.{domain}` TXT 记录                            | RFC 8555 §8.4 |
| `TlsAlpn01ChallengeValidator` | TLS-ALPN-01 | 连接目标域名并验证 `acme-tls/1` ALPN 证书扩展                       | RFC 8737      |

挑战验证由 `ChallengeValidationBackgroundService`（`Acme.Server.Host`）后台异步触发；验证结果写回 `Challenge` → `Authorization` → `Order` 聚合根状态机。

### `INonceRepository`（Acme.Server.Core）

Nonce 存取接口，防止请求重放（RFC 8555 §7.2）。  
方法：`GenerateAsync(ct)` → `string nonce`；`ConsumeAsync(nonce, ct)` → `bool`（已用或不存在返回 `false`）。  
默认实现 `InMemoryNonceRepository`（`ConcurrentDictionary` + TTL 滑动过期）。  
扩展点 `IDistributedNonceRepository` 留供分布式多实例场景替换（如 Redis）。

### `IExternalAccountBindingService`（Acme.Server.Core）

外部账户绑定验证接口（RFC 8555 §7.3.4）。  
方法：`ValidateEabAsync(kid, hmacKey, eabJws, ct)` → `bool`；验证 EAB JWS 使用 `kid` 对应的 HMAC 密钥签名是否正确。

### 服务端仓储接口（Acme.Server.Core）

| 接口                           | 管理实体        | 默认实现（Server.Infrastructure，EFCore + PostgreSQL） |
| ------------------------------ | --------------- | ------------------------------------------------------ |
| `IServerAccountRepository`     | `Account`       | `EfCoreAccountRepository`                              |
| `IOrderRepository`             | `Order`         | `EfCoreOrderRepository`                                |
| `IAuthorizationRepository`     | `Authorization` | `EfCoreAuthorizationRepository`                        |
| `IServerCertificateRepository` | `Certificate`   | `EfCoreCertificateRepository`                          |

---

## 证书申请完整数据流

```
客户端                                         ACME 服务器
  │                                                │
  │── GET /directory ─────────────────────────────▶│  获取目录（端点 URL 集合）
  │◀─ DirectoryModel ────────────────────────────── │
  │                                                │
  │── HEAD /acme/new-nonce ───────────────────────▶│  获取 Nonce（防重放）
  │◀─ Replay-Nonce ──────────────────────────────── │
  │                                                │
  │── POST /acme/new-account（JWS 签名）──────────▶│  创建/查询账户
  │◀─ AccountModel + Location（Kid）─────────────── │
  │                                                │
  │── POST /acme/new-order（JWS + Kid）───────────▶│  提交证书订单
  │◀─ OrderModel（pending）────────────────────────│
  │                                                │
  │── POST /acme/authz/{id}（获取授权详情）────────▶│  查询域名授权
  │◀─ AuthorizationModel（challenges）─────────────│
  │                                                │
  │  [客户端完成 HTTP-01 / DNS-01 / TLS-ALPN-01]  │
  │                                                │
  │── POST /acme/chall/{id}（回应挑战）───────────▶│  通知服务器验证
  │◀─ ChallengeModel（valid）──────────────────────│
  │                                                │
  │── POST /acme/order/{id}/finalize（附 CSR）─────▶│  终结订单
  │◀─ OrderModel（valid / processing）──────────── │
  │                                                │
  │── POST /acme/cert/{id}（下载证书）────────────▶│  收集证书链
  │◀─ PEM 证书链（string[]）────────────────────── │
```

---

## 服务端请求处理数据流

```
ACME 客户端 POST 请求
        │
        ▼
JwsVerificationFilter（IEndpointFilter，Acme.Server.Protocol）
  ├─ 反序列化 Flattened JWS（protected / payload / signature）
  ├─ INonceRepository.ConsumeAsync() — 验证并消费 Nonce（防重放）
  ├─ 校验 JWS protected.url 字段 == 当前请求 URL
  └─ IJwsVerifier.VerifyAsync() — 验证签名与账户公钥
        │
        ▼
Minimal API 端点 Handler（Acme.Server.Protocol）
  └─ 编排领域服务，更新 Account / Order / Authorization / Challenge 聚合根
        │
        ▼
   [证书终结] ICAProvider.IssueCertificateAsync()
        │
        ▼
   响应（含 Replay-Nonce: <新 Nonce> 响应头）

─── 后台挑战验证（异步）────────────────────────────────────────────
ChallengeValidationBackgroundService（Acme.Server.Host）
  └─ 轮询 Status == pending 的 Challenge
        │
        ▼
   IServerChallengeValidator.ValidateAsync()
   ├─ 验证成功 → Challenge.status = valid
   │             Authorization.status = valid
   │             [所有 Authz valid] → Order.status = ready
   └─ 验证失败 → Challenge.status = invalid（记录 error 字段）
```

---

## JWS 请求封装

每个 POST 请求体均为 Flattened JWS Serialization（RFC 7515），结构如下：

```json
{
  "protected": "<Base64Url(header)>",
  "payload": "<Base64Url(body) | ''>",
  "signature": "<Base64Url(sig)>"
}
```

其中 `header` 包含 `alg`、`nonce`、`url` 及首次请求的 `jwk`（后续请求改用 `kid`）。

---

## 主要 NuGet 依赖

| 包                                  | 版本   | 用途                                  |
| ----------------------------------- | ------ | ------------------------------------- |
| `BouncyCastle.Cryptography`         | 2.6.2  | HMAC、EC/RSA 签名底层实现             |
| `Microsoft.IdentityModel.Tokens`    | 8.15.0 | JWK 标准类型、JWT 辅助                |
| `Microsoft.Extensions.Http`         | 10.0.1 | `IHttpClientFactory`，HTTP 连接池管理 |
| `Microsoft.Extensions.Logging`      | 10.0.1 | 结构化日志接口                        |
| `Microsoft.Extensions.Localization` | 10.0.1 | 多语言资源（UI 错误消息 i18n）        |
| `System.ComponentModel.Annotations` | 5.0.0  | `[Display]` 特性，枚举显示名称        |

---

## 多框架支持策略

`common.props` 统一配置多目标框架：

```xml
<TargetFrameworks>net8.0;net9.0;net10.0</TargetFrameworks>
```

基础层（`Acme.Base`、`Acme.Shared.Protocol`、`Acme.Shared.Core`）额外支持：

```xml
<TargetFrameworks>netstandard2.0;netstandard2.1;net8.0;net9.0;net10.0</TargetFrameworks>
```

以保证类库可被旧版 .NET Framework 5/6 或 Xamarin 等平台引用。  
SDK 版本由 `global.json` 锁定为 .NET 10（`rollForward: latestMajor`）。

---

## RFC 文档索引

完整的中英文 RFC 翻译见 [`docs/rfc/`](rfc/)。

| RFC                               | 标题                         | 关联模块                |
| --------------------------------- | ---------------------------- | ----------------------- |
| [RFC 8555](rfc/acme/rfc8555.html) | ACME 协议核心                | 全局                    |
| [RFC 8737](rfc/acme/rfc8737.html) | TLS-ALPN-01 挑战             | 客户端/服务端挑战验证器 |
| [RFC 8738](rfc/acme/rfc8738.html) | IP 标识符验证                | 订单/授权模型           |
| [RFC 8739](rfc/acme/rfc8739.html) | Short-Term Automatic Renewal | 续期策略                |
| [RFC 9115](rfc/acme/rfc9115.html) | 代理订阅                     | 账户扩展                |
| [RFC 9447](rfc/acme/rfc9447.html) | 订单列表                     | `OrderListModel`        |
| [RFC 9448](rfc/acme/rfc9448.html) | 委托标识符                   | 标识符模型              |
| [RFC 7515](rfc/jws/rfc7515.html)  | JWS                          | `IJwkSigner`、JWS 封装  |
| [RFC 7517](rfc/jws/rfc7517.html)  | JWK                          | `IJsonWebKey`           |
| [RFC 7518](rfc/jws/rfc7518.html)  | JWA（算法）                  | `JsonWebKeyAlgorithms`  |
| [RFC 7638](rfc/jws/rfc7638.html)  | JWK Thumbprint               | JWK 指纹计算            |

---

## 项目文件树

```
CSharp-Acme/
├── src/
│   ├── Shared/                                          # 共享库（客户端与服务端均可引用）
│   │   ├── Acme.Protocol.Shared/                        # ACME 协议及加密相关共享库
│   │   │   ├── HttpModels/                              # ACME HTTP 请求/响应模型
│   │   │   ├── Enums/                                   # 协议枚举
│   │   │   ├── Exceptions/                              # 异常体系（AcmeException 基类）
│   │   │   ├── Crypto/                                  # JWK / JWS 签名与验签 / PEM
│   │   │   │   ├── Jwk/                                 # IJsonWebKey / RsaJsonWebKey / EcJsonWebKey
│   │   │   │   └── Jws/                                 # IJwkSigner / IJwsVerifier / DefaultJwkSigner
│   │   │   ├── X509/                                    # CertUtils / CsrUtils / KeyUtils / PemFormatter
│   │   │   └── Utils/                                   # DirectoryUrlNormalizer / HexConverter
│   │   ├── Acme.Core.Shared/                            # 客户端与服务端通用 DDD 基础库
│   │   │   ├── Domain/                                  # IAggregateRoot / IEntity / IDomainEvent
│   │   │   ├── Repositories/                            # IRepository<T> / IUnitOfWork
│   │   │   ├── Events/                                  # IDomainEventHandler<T> / IDomainEventDispatcher
│   │   │   └── Utils/                                   # AuditedEntity / 通用工具扩展
│   │   └── Acme.Protocol.Client/                        # ACME 客户端协议实现（位于 Shared，供多层引用）
│   │       ├── Protocol/                                # AcmeProtocolClient / AcmeHttpClient
│   │       ├── Contexts/                                # IAcmeAccountContext / AcmeAccountContext
│   │       ├── Options/                                 # AcmeProtocolClientOptions
│   │       └── Extensions/                              # DI 注册扩展
│   ├── Client/                                          # 客户端应用层
│   │   ├── Acme.Client.Core/                            # 客户端领域核心（依赖 Acme.Core.Shared）
│   │   │   ├── Aggregates/                              # Certificate / Account 聚合根
│   │   │   ├── Enums/                                   # ChallengeMode / CertificateStatus
│   │   │   ├── Repositories/                            # ICertificateRepository / IAccountRepository
│   │   │   └── Providers/                               # IDnsProvider / IChallengeResponder
│   │   ├── Acme.Client.Application/                     # 应用层（CQRS Handler / DTO）
│   │   │   ├── Commands/                                # RequestCertificateCommand / RenewCertificateCommand
│   │   │   ├── Queries/                                 # ListCertificatesQuery / GetCertificateQuery
│   │   │   └── Services/                                # ICertificateService
│   │   ├── Acme.Client.Infrastructure/                  # 基础设施层
│   │   │   ├── Repositories/                            # FileCertificateRepository / FileAccountRepository
│   │   │   ├── Responders/                              # Http01 / Dns01 / TlsAlpn01 ChallengeResponder
│   │   │   └── Extensions/                              # DI 注册扩展（AddAcmeClientCore）
│   │   ├── Acme.Client.Host/                            # ASP.NET Core 宿主（HTTP/2 API + BackgroundService）
│   │   │   ├── Controllers/                             # 证书/账户/设置 HTTP 端点
│   │   │   ├── BackgroundServices/                      # CertificateRenewalBackgroundService
│   │   │   ├── Options/                                 # AcmeClientHostOptions
│   │   │   └── Extensions/                              # DI 注册扩展（AddAcmeClientHost）
│   │   ├── Acme.Client.Cli/                             # Spectre.Console.Cli 命令行工具
│   │   │   ├── Commands/                                # request / renew / list / export 命令
│   │   │   └── Services/                                # HTTP 客户端工厂
│   │   └── Acme.Client.Web/                    # Vue 3 + TypeScript SPA（前端独立项目）
│   │       ├── src/
│   │       │   ├── pages/                               # Certificates / Accounts / Settings 页面
│   │       │   ├── components/                          # 复用 UI 组件
│   │       │   ├── hooks/                               # useApi / useAuth / useCertificates Hook
│   │       │   ├── services/                            # api.ts（HTTP 客户端拦截器）
│   │       │   ├── stores/                              # Pinia 状态管理（证书、账户、认证）
│   │       │   ├── App.vue                              # 根组件
│   │       │   └── main.ts                              # 入口
│   │       ├── index.html                               # HTML 模板
│   │       ├── vite.config.ts                           # Vite 构建配置
│   │       ├── tsconfig.json                            # TypeScript 配置
│   │       └── package.json                             # Node.js 依赖
│   ├── Server/                                          # 服务端应用层
│   │   ├── Acme.Server.Core/                            # 服务端领域核心（依赖 Acme.Core.Shared）
│   │   │   ├── Aggregates/                              # Account / Order / Authorization / Challenge / Certificate
│   │   │   ├── Providers/                               # ICAProvider / IServerChallengeValidator
│   │   │   ├── Repositories/                            # IServerAccountRepository / IOrderRepository 等
│   │   │   └── Services/                                # INonceRepository / IExternalAccountBindingService
│   │   ├── Acme.Server.Infrastructure/                  # 基础设施层
│   │   │   ├── CA/                                      # BouncyCastleCAProvider
│   │   │   ├── Data/                                    # AcmeServerDbContext（EFCore + PostgreSQL）
│   │   │   ├── Repositories/                            # EfCoreAccountRepository / EfCoreOrderRepository 等
│   │   │   ├── Nonce/                                   # InMemoryNonceRepository
│   │   │   ├── Validators/                              # Http01 / Dns01 / TlsAlpn01 ChallengeValidator
│   │   │   └── Extensions/                              # DI 注册扩展（AddAcmeServerInfrastructure）
│   │   ├── Acme.Server.Protocol/                        # 服务端协议层（Minimal API 端点）
│   │   │   ├── Endpoints/                               # DirectoryEndpoints / NonceEndpoints ...
│   │   │   ├── Filters/                                 # JwsVerificationFilter（IEndpointFilter）
│   │   │   └── Extensions/                              # AcmeEndpointRouteBuilderExtensions（MapAcmeServer）
│   │   ├── Acme.Server.Host/                            # ASP.NET Core 宿主集成
│   │   |   ├── BackgroundServices/                      # ChallengeValidationBackgroundService
│   │   |   ├── Options/                                 # AcmeServerOptions
│   │   |   └── Extensions/                              # AcmeServerServiceCollectionExtensions（AddAcmeServer）
│   │   └── DnsProviders/                                # 服务端 DNS 验证插件
│   │       └── Acme.Server.Dns.Provider.Aliyun/         # 阿里云 DNS 插件（验证实现）
└── tests/
    ├── Shared/
    │   ├── Acme.Protocol.Shared.Tests/                  # JWK / JWS / X.509 单元测试
    │   └── Acme.Core.Shared.Tests/                      # DDD 基础接口单元测试
    ├── Client/
    │   ├── Acme.Protocol.Client.Tests/                  # 协议客户端单元测试 + LE Staging 集成测试
    │   ├── Acme.Client.Core.Tests/                      # 客户端聚合根单元测试
    │   └── Acme.Client.Protocol.Tests/                  # 端到端集成测试
    └── Server/
        ├── Acme.Server.Core.Tests/                      # 服务端聚合根状态机单元测试
        ├── Acme.Server.Infrastructure.Tests/            # CA / EFCore 仓储 / 挑战验证器单元测试
        ├── Acme.Server.Protocol.Tests/                  # JwsVerificationFilter + 端点 HTTP 集成测试
        └── Acme.Server.Integration.Tests/               # 客户端 ↔ 服务端全流程端到端测试
```
