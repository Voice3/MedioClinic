using CMS.Base;
using CMS.DataEngine;
using CMS.MediaLibrary;
using Core.Configuration;
using Kentico.Content.Web.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XperienceAdapter.Models;

namespace XperienceAdapter.Repositories
{
	public class MediaFileRepository : IMediaFileRepository
	{
		private readonly IMediaLibraryInfoProvider _mediaLibraryInfoProvider;

		private readonly IMediaFileInfoProvider _mediaFileInfoProvider;

		private readonly ISiteService _siteService;

		private readonly IMediaFileUrlRetriever _mediaFileUrlRetriever;

		private readonly IOptionsMonitor<XperienceOptions> _optionsMonitor;

		public MediaFileRepository(
			IMediaLibraryInfoProvider mediaLibraryInfoProvider,
			IMediaFileInfoProvider mediaFileInfoProvider,
			ISiteService siteService,
			IMediaFileUrlRetriever mediaFileUrlRetriever,
			IOptionsMonitor<XperienceOptions> optionsMonitor)
		{
			_mediaLibraryInfoProvider = mediaLibraryInfoProvider ?? throw new ArgumentNullException(nameof(mediaLibraryInfoProvider));
			_mediaFileInfoProvider = mediaFileInfoProvider ?? throw new ArgumentNullException(nameof(mediaFileInfoProvider));
			_siteService = siteService ?? throw new ArgumentNullException(nameof(siteService));
			_mediaFileUrlRetriever = mediaFileUrlRetriever ?? throw new ArgumentNullException(nameof(mediaFileUrlRetriever));
			_optionsMonitor = optionsMonitor ?? throw new ArgumentNullException(nameof(optionsMonitor));
		}

		private int GetLibraryId(string mediaLibraryName)
		{
			if (string.IsNullOrEmpty(mediaLibraryName))
			{
				throw new ArgumentException($"The {nameof(mediaLibraryName)} parameter must a non-empty string.");
			}

			return _mediaLibraryInfoProvider
				.Get(mediaLibraryName, _siteService.CurrentSite.SiteID)
				.LibraryID;
		}

		private MediaLibraryFile MapDtoProperties(MediaFileInfo mediaFileInfo) => new MediaLibraryFile()
	{
		Guid = mediaFileInfo.FileGUID,
		Name = mediaFileInfo.FileTitle,
		Extension = mediaFileInfo.FileExtension,
		MediaFileUrl = _mediaFileUrlRetriever.Retrieve(mediaFileInfo),
		IsImage = _optionsMonitor.CurrentValue?.MediaLibraryOptions?.AllowedImageExtensions?.Contains(mediaFileInfo.FileExtension) == true,
		Width = mediaFileInfo.FileImageWidth,
		Height = mediaFileInfo.FileImageHeight
	};


		private async Task<IEnumerable<MediaLibraryFile>> GetResultAsync(Func<ObjectQuery<MediaFileInfo>, ObjectQuery<MediaFileInfo>>? filter,
	CancellationToken? cancellationToken)
		{
			var query = _mediaFileInfoProvider.Get();

			if (filter != null)
			{
				query = filter(query);
			}

			return (await query.GetEnumerableTypedResultAsync(cancellationToken: cancellationToken))
				.Select(item => MapDtoProperties(item));
		}

		public async Task<IEnumerable<MediaLibraryFile>> GetMediaFilesAsync(string mediaLibraryName, string path, CancellationToken? cancellationToken = default)
		{
			var libraryId = GetLibraryId(mediaLibraryName);

			return await GetResultAsync(baseQuery =>
				baseQuery
					.WhereStartsWith("FilePath", path),
				cancellationToken);
		}

		public async Task<MediaLibraryFile?> GetMediaFileAsync(Guid fileGuid, CancellationToken? cancellationToken = default)
		{
			var mediaFileInfo = await _mediaFileInfoProvider.GetAsync(fileGuid, _siteService.CurrentSite.SiteID, cancellationToken);

			return mediaFileInfo != null ? MapDtoProperties(mediaFileInfo) : null;
		}

		public async Task<IEnumerable<MediaLibraryFile>> GetAllAsync(CancellationToken? cancellationToken = default) =>
			await GetResultAsync(null, cancellationToken: cancellationToken);

		public IEnumerable<MediaLibraryFile> GetAll() => GetAllAsync().GetAwaiter().GetResult();
	}
}
