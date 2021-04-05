using System;
using System.Collections.Generic;
using System.Text;
using XperienceAdapter.Repositories;
using XperienceAdapter.Services;

namespace Business.Repositories
{
	public class SocialLinkRepository : BasePageRepository<Models.SocialLink, CMS.DocumentEngine.Types.MedioClinic.SocialLink>
	{
		public override void MapDtoProperties(CMS.DocumentEngine.Types.MedioClinic.SocialLink page, Models.SocialLink dto)
		{
			dto.Url = page?.Url;

			if (page?.Fields?.Icon != null)
			{
				dto.IconUrl = _repositoryServices.PageAttachmentUrlRetriever.Retrieve(page?.Fields?.Icon);
			}
		}

		public SocialLinkRepository(IRepositoryServices repositoryDependencies) : base(repositoryDependencies)
		{
		}
	}
}
