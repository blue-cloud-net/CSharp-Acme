# 开发规范

## 一、环境要求

| 工具     | 版本要求                                | 说明                                   |
| -------- | --------------------------------------- | -------------------------------------- |
| .NET SDK | 10.0（`global.json` 锁定）              | `rollForward: latestMajor`，允许预览版 |
| C#       | `latest`（`common.props` 设置）         | 使用最新语言特性                       |
| IDE      | VS 2022+ / Rider / VS Code + C# Dev Kit | 均支持多目标框架调试                   |

### 常用命令

```bash
# 构建
dotnet build

# 运行测试
dotnet test

# 代码格式化（检查）
dotnet format --verify-no-changes

# 代码格式化（修复）
dotnet format

# 打包
dotnet pack -c Release
```

---

## 二、代码风格

### 2.1 文件与命名空间

- **文件作用域命名空间**（`namespace Acme.Foo;`），禁用块级命名空间。
- **一个文件一个顶级类型**，文件名与类型名完全一致。
- 遵守 `.editorconfig` 中的缩进与换行规则。

### 2.2 命名约定

| 类别                   | 规范             | 示例                  |
| ---------------------- | ---------------- | --------------------- |
| 类型、属性、方法、事件 | PascalCase       | `AcmeProtocolClient`  |
| 局部变量、参数         | camelCase        | `cancellationToken`   |
| 常量、静态只读字段     | PascalCase       | `DefaultTimeout`      |
| 私有字段               | `_camelCase`     | `_httpClient`         |
| 接口                   | `I` + PascalCase | `IAcmeProtocolClient` |

### 2.3 可空引用类型

项目全量启用（`<Nullable>enable</Nullable>`）：

- 所有公共 API 参数在方法体内做 `ArgumentNullException.ThrowIfNull` 校验。
- 返回可能为 `null` 的值需明确用 `?` 标注。
- 禁止使用 `!`（null-forgiving）掩盖潜在问题。

---

## 三、语言与注释规范

### 3.1 代码注释

- **注释语言：中文**。
- 公共 API 必须有 XML 文档注释（`<summary>`、`<param>`、`<returns>`、`<exception>`）。
- 实现类/方法优先使用 `<inheritdoc />` 继承接口注释，仅在有特殊实现说明时覆写。
- 复杂逻辑写简短注释说明**意图**，而非重复代码过程。

```csharp
/// <summary>
/// 获取 ACME 服务器目录。
/// </summary>
/// <remarks>
/// 参见 <see href="https://datatracker.ietf.org/doc/html/rfc8555#section-7.1.1">RFC 8555 §7.1.1</see>。
/// </remarks>
```

### 3.2 RFC 引用格式

```xml
<see href="https://datatracker.ietf.org/doc/html/rfc8555#section-7.1">RFC 8555 §7.1</see>
```

### 3.3 日志消息

- **日志语言：英文**，句子结尾必须带句号（`.`）。
- 使用 `ILogger<T>` 结构化日志，**参数化消息模板**，**禁止**字符串拼接。
- 不记录敏感信息、PII（个人身份信息）、密钥或证书私钥。

```csharp
// ✅ 正确：结构化日志，参数化模板，英文，句尾带句号
_logger.LogInformation("Creating ACME account for contact {Contact}.", contact);
_logger.LogError(ex, "Failed to fetch directory from {Url}.", directoryUrl);

// ❌ 错误：字符串拼接
_logger.LogInformation("Creating account for: " + contact);

// ❌ 错误：中文日志
_logger.LogInformation("正在创建账户。");
```

### 3.4 异常消息

- **异常消息语言：英文**，句子结尾必须带句号（`.`）。
- 异常消息应清晰描述**发生了什么**，而非后续如何处理。

```csharp
// ✅ 正确
throw new InvalidOperationException("Account has not been initialized.");
throw new ArgumentException("Identifier type must be 'dns' or 'ip'.", nameof(identifier));

// ❌ 错误：缺少句号
throw new InvalidOperationException("Account has not been initialized");
```

### 3.5 UI 错误消息与国际化

- 返回给 UI 的错误消息必须走多语言资源（`IStringLocalizer<T>` 或 `IStringLocalizerFactory`）。
- UI 可见的所有文字（包括验证提示、状态描述等）走 i18n 资源文件，不硬编码中文或英文字符串。
- 资源文件命名：`{ClassName}.{locale}.resx`，默认中性文化（`zh-CN`）。

---

## 四、异步规范

- 所有 I/O 操作使用 `async/await`，禁止 `.Result` / `.Wait()` 阻塞调用。
- **所有公开 `async` 方法必须接受 `CancellationToken ct = default` 参数**，放在参数列表末尾。
- 库代码（非应用入口）调用 `await` 时追加 `.ConfigureAwait(false)`，避免死锁。
- `ValueTask`/`Task` 的选择：高频无分配场景用 `ValueTask`，一般情况用 `Task`。

```csharp
// ✅ 正确
public async Task<DirectoryModel> GetDirectoryAsync(CancellationToken ct = default)
{
    var response = await _httpClient.GetAsync(_directoryUrl, ct).ConfigureAwait(false);
    // ...
}

// ❌ 错误：缺少 CancellationToken
public async Task<DirectoryModel> GetDirectoryAsync()
```

---

## 五、依赖注入

- 使用 .NET 内置 DI（`Microsoft.Extensions.DependencyInjection`），禁止使用第三方容器。
- 通过**接口**编程，对外暴露最小必要接口。
- 生命周期优先顺序：`Singleton` > `Scoped` > `Transient`，按实际状态需求选择。
- 提供 `IServiceCollection` 扩展方法（`AddXxx`），避免要求消费方手动注册内部依赖。

---

## 六、错误处理

- **不吞异常**：catch 块必须记录日志或重新抛出，禁止空 catch。
- 使用项目分层异常体系（见下），向上层暴露有意义的错误类型。
- 对可预期失败（如资源不存在、账户未就绪）使用具体派生异常，而非通用 `Exception`。
- Web 层返回 `ProblemDetails`（RFC 7807），不暴露堆栈信息到外部。

### 异常体系

```
AcmeException（基类）
├── MalformedRequestException     # 请求格式错误
├── UnauthorizedException         # 鉴权失败
├── AccountDoesNotExistException  # 账户不存在
├── OrderNotReadyException        # 订单未就绪
└── ...（其余派生类）
```

---

## 七、测试约定

### 7.1 框架与工具

| 工具             | 版本  | 用途       |
| ---------------- | ----- | ---------- |
| xUnit            | 2.9+  | 测试框架   |
| FluentAssertions | 8.x   | 流畅断言   |
| Moq              | 4.20+ | Mock 对象  |
| coverlet         | 6.x   | 代码覆盖率 |

### 7.2 命名规范

```
MethodName_Should_ExpectedBehavior_When_Condition
```

示例：

```csharp
GetDisplayName_Should_Return_DisplayName_When_DisplayAttributeExists
CreateOrderAsync_Should_Throw_ArgumentNullException_When_IdentifiersIsEmpty
```

### 7.3 测试模式（AAA）

```csharp
[Fact]
public void ToEnum_Should_Return_EnumValue_When_ValidStringIsProvided()
{
    // Arrange
    var value = "Value1";

    // Act
    var result = value.ToEnum<TestEnum>();

    // Assert
    result.Should().Be(TestEnum.Value1);
}
```

### 7.4 参数化测试

优先使用 `[Theory]` + `[InlineData]` 覆盖多个输入：

```csharp
[Theory]
[InlineData("Value1", TestEnum.Value1)]
[InlineData("Value2", TestEnum.Value2)]
public void ToEnum_Should_Return_CorrectEnumValue_When_ValidStringsAreProvided(
    string value, TestEnum expected)
{
    value.ToEnum<TestEnum>().Should().Be(expected);
}
```

### 7.5 覆盖要求

- **必须**覆盖：正常路径、边界值、异常/错误路径。
- **关键业务分支**必须有对应测试。
- 目标覆盖率：**≥ 80%**（通过 coverlet 统计）。
- 单测原子、可重复，与网络/文件系统等外部依赖隔离（使用 Moq）。
- 集成测试打 `[Trait("Category", "Integration")]`，CI 默认跳过，按需启用。

---

## 八、安全清单

- [ ] 所有外部输入（HTTP 请求体、配置值）在系统边界做参数校验。
- [ ] 禁止字符串拼接 SQL；若使用数据库，只用参数化查询。
- [ ] 不将密钥、私钥、Token、凭据写入日志或代码。
- [ ] 配置敏感项通过 `IOptions<T>` + 环境变量注入，不硬编码到源码。
- [ ] HTTP 响应不暴露堆栈跟踪、内部异常详情到外部。
- [ ] 第三方依赖最小化，定期更新（`dependabot` 或手动审查）。

---

## 九、配置与机密

- 配置使用 `IOptions<T>` 绑定，支持 `appsettings.json` 与环境变量覆盖（环境变量优先级更高）。
- 环境差异配置：`appsettings.Development.json`、`appsettings.Production.json` 等。
- **绝不**将 API 密钥、证书私钥、数据库密码等机密提交到版本库。

---

## 十、提交规范

遵循 [Conventional Commits](https://www.conventionalcommits.org/)：

```
<type>(<scope>): <简短描述>

[可选：正文说明动机与影响]

[可选：Breaking Change / Fixes #issue]
```

| 类型       | 用途               |
| ---------- | ------------------ |
| `feat`     | 新功能             |
| `fix`      | Bug 修复           |
| `docs`     | 文档变更           |
| `refactor` | 重构（不影响行为） |
| `test`     | 测试相关           |
| `chore`    | 构建/配置/工具链   |
| `perf`     | 性能优化           |
| `ci`       | CI/CD 流水线       |

示例：

```
feat(client): add certificate revocation support per RFC 8555 §7.6

Implements CertificateRevokeAsync on IAcmeProtocolClient.
Includes unit tests and updates to the exception hierarchy.
```
