# CSharp-Acme

[RFC 8555](https://datatracker.ietf.org/doc/html/rfc8555) ACME 协议的 C# 完整实现，支持 TLS/SSL 证书自动申请、续期、吊销，提供**客户端协议库、客户端应用与服务端实现**。

## 特性

- **完整 ACME 协议支持** — RFC 8555 全流程，含 RFC 8739（ARI）、RFC 9447（订单列表）等扩展
- **客户端与服务端** — 客户端 C/S 架构（CLI + Vue Web + HTTP/2 Host）；服务端可嵌入 ASP.NET Core
- **多种挑战验证** — HTTP-01、DNS-01、TLS-ALPN-01，DNS 支持阿里云等插件化实现
- **插件化 CA** — 服务端内置 BouncyCastle 自建根 CA，可替换为外部 PKI
- **多目标框架** — `net8.0`、`net9.0`、`net10.0`（共享库额外支持 `netstandard2.0/2.1`）

## 快速开始

### 先决条件

- .NET 10 SDK（`global.json` 锁定版本）
- PostgreSQL（服务端持久化）

### 构建

```bash
git clone https://github.com/blue-cloud-net/CSharp-Acme.git
cd CSharp-Acme
dotnet restore
dotnet build
dotnet test
```

### 三个核心部分

#### 1. 客户端协议库 (`Acme.Protocol.Client`)

提供 ACME 协议完整实现，支持 RFC 8555 全流程（账户、订单、授权、挑战、证书）。

```csharp
// 注册服务
services.AddAcmeProtocolClient(options =>
{
    options.DirectoryUrl = "https://acme-staging-v02.api.letsencrypt.org/directory";
});

// 创建协议客户端实例
var client = factory.CreateClient(accountContext);
var order = await client.CreateOrderAsync(new OrderCreateModel { ... });
await client.AnswerChallengeAsync(challengeUrl, ...);
var cert = await client.CollectOrderCertificateAsync(finalizeUrl);
```

#### 2. 客户端应用 (`Acme.Client.*`)

提供 C/S 架构（CLI 客户端 + HTTP/2 Host），支持证书申请、续期、吊销、导出管理。

- **Vue Web UI** — 前后端分离，可视化管理界面，查询、续期、导出证书
- **HTTP/2 Host** — ASP.NET Core 宿主，RESTful API，支持 API Key 认证，触发后台续期任务
- **CLI 工具** — 命令行快捷操作，自包含可执行文件

#### 3. 服务端 (`Acme.Server.*`)

嵌入 ASP.NET Core 的完整 ACME 服务实现，支持证书颁发、验证、吊销。

```csharp
// Program.cs
builder.Services.AddAcmeServer(options =>
{
    options.RoutePrefix = "/";          // 默认路由前缀
    options.CaPemPath = "ca.pem";       // 留空则自动生成自签根 CA
});

app.MapAcmeServer();                    // 注册所有 ACME 端点
```

## 项目结构

完整的项目分层与文件树见 [架构文档](docs/architecture.md#项目文件树)。

| 目录                       | 说明                                                                         |
| -------------------------- | ---------------------------------------------------------------------------- |
| `src/Shared/`              | 共享库（`Acme.Protocol.Shared`、`Acme.Core.Shared`、`Acme.Protocol.Client`） |
| `src/Client/`              | 客户端应用层（Core / Application / Infrastructure / Host / CLI / Web）       |
| `src/Server/`              | 服务端（Core / Infrastructure / Protocol / Host / DnsProviders）             |
| `src/DnsProviders/`        | 客户端 DNS 提供商插件（阿里云等）                                            |
| `src/Server/DnsProviders/` | 服务端 DNS 验证插件（阿里云等）                                              |
| `tests/`                   | 单元测试与集成测试（Shared / Client / Server）                               |
| `docs/`                    | 架构文档、开发规范、路线图、RFC 中文翻译                                     |

## 文档

| 文档                                  | 内容                                                                   |
| ------------------------------------- | ---------------------------------------------------------------------- |
| [架构文档](docs/architecture.md)      | 项目定位、分层图、各层职责、关键接口、完整数据流、项目文件树、RFC 索引 |
| [开发路线图](docs/roadmap.md)         | 各阶段进度（复选框列表）、里程碑版本                                   |
| [开发规范](docs/development-guide.md) | 代码风格、命名约定、异步/日志/测试/安全完整规范                        |
| [RFC 翻译](docs/rfc/)                 | RFC 8555、7515 等中文翻译                                              |

## 主要依赖

| 包                                      | 用途                           |
| --------------------------------------- | ------------------------------ |
| `BouncyCastle.Cryptography` 2.6         | HMAC、EC/RSA 签名、CA 证书签发 |
| `Microsoft.IdentityModel.Tokens` 8      | JWK 标准类型                   |
| `Microsoft.Extensions.*` 10             | DI、HTTP、日志                 |
| `Npgsql.EntityFrameworkCore.PostgreSQL` | 服务端 PostgreSQL 持久化       |

## 贡献

欢迎 PR 与 Issue！请先阅读 [开发规范](docs/development-guide.md)。

```bash
# 运行测试
dotnet test

# 格式检查
dotnet format --verify-no-changes
```

## 许可证

[Apache License 2.0](LICENSE)
