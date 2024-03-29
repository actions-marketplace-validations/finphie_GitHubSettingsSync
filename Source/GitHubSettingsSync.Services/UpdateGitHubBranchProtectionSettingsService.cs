﻿using GitHubSettingsSync.Repositories;
using Microsoft.Extensions.Logging;

namespace GitHubSettingsSync.Services;

/// <summary>
/// GitHubブランチ保護設定の更新を行うクラスです。
/// </summary>
public sealed partial class UpdateGitHubBranchProtectionSettingsService : IUpdateGitHubBranchProtectionSettingsService
{
    readonly ILogger<UpdateGitHubBranchProtectionSettingsService> _logger;
    readonly IGitHubRepositoryBranchProtectionSettingsRepository _gitHub;

    /// <summary>
    /// <see cref="UpdateGitHubBranchProtectionSettingsService"/>クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="logger">ロガー。</param>
    /// <param name="gitHub">GitHubブランチ保護設定の操作を行うクラスのインスタンス。</param>
    /// <exception cref="ArgumentNullException"><paramref name="logger"/>または<paramref name="gitHub"/>がnullです。</exception>
    public UpdateGitHubBranchProtectionSettingsService(ILogger<UpdateGitHubBranchProtectionSettingsService> logger, IGitHubRepositoryBranchProtectionSettingsRepository gitHub)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(gitHub);

        _logger = logger;
        _gitHub = gitHub;
    }

    /// <inheritdoc/>
    public async ValueTask ExecuteAsync(string owner, string repositoryName, string branch, GitHubBranchProtectionSettings settings, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(repositoryName);
        ArgumentException.ThrowIfNullOrEmpty(branch);

        Starting();

        try
        {
            await _gitHub.Update(new(owner, repositoryName, branch, settings), cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Error(ex);
            throw;
        }
    }

    [LoggerMessage(EventId = 1000, Level = LogLevel.Information, Message = $"{nameof(UpdateGitHubBranchProtectionSettingsService)} is starting.")]
    partial void Starting();

    [LoggerMessage(EventId = 9000, Level = LogLevel.Error)]
    partial void Error(Exception ex);
}
