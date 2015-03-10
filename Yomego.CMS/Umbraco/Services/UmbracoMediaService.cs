using System;
using System.Linq;
using System.Reflection;
using System.Web;
using Humanizer;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Yomego.CMS.Core.Umbraco.Model;

namespace Yomego.CMS.Umbraco.Services
{
    public class UmbracoMediaService : MediaService
    {
        private IMediaService _mediaService { get; set; }

        public IMediaService MediaService
        {
            get
            {
                if (_mediaService == null)
                {
                    _mediaService = ApplicationContext.Current.Services.MediaService;
                }

                return _mediaService;
            }
        }

        private MediaItem PopulateMedia(IMedia media)
        {
            MediaItem mediaItem = null;

            if (media != null)
            {
                mediaItem = new MediaItem
                {
                    Name = media.Name,
                    Id = media.Id,
                    ParentId = media.ParentId,
                    Path = media.Path,
                    Level = media.Level
                };
            }

            return mediaItem;
        }

        public override MediaItem Get(int id, bool includeChildren = false)
        {
            var media = MediaService.GetById(id);

            var mediaItem = PopulateMedia(media);

            if (mediaItem != null)
            {
                if (includeChildren)
                {
                    var children = MediaService.GetChildren(id);

                    if (children != null && children.Any())
                    {
                        mediaItem.Children = children.Select(PopulateMedia).Where(i => i != null).ToList();
                    }
                }
            }

            return mediaItem;
        }

        public override int Save(MediaItem mediaItem)
        {
            // [ML] - I removed the publish children also as this seemed excessive

            return SaveMedia(mediaItem);
        }

        #region Save Implementation

        private int SaveMedia(MediaItem mediaItem)
        {
            IMedia media = null;

            if (mediaItem.Id == 0) // content item is new so create Document
            {
                var typeName = (mediaItem.GetType().Name.Dehumanize());

                media = ApplicationContext.Current.Services.MediaService.CreateMedia(mediaItem.Name,
                                                                                     mediaItem.ParentId,
                                                                                     Char.ToLowerInvariant(typeName[0]) + typeName.Substring(1));
            }
            else // content item already exists, so load it
            {
                media = ApplicationContext.Current.Services.MediaService.GetById(mediaItem.Id);
            }

            mediaItem.Id = media.Id;

            var entityType = mediaItem.GetType();

            foreach (var p in media.Properties)
            {
                var propertyInfo = entityType.GetProperty(p.Alias, BindingFlags.IgnoreCase | BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                
                if (propertyInfo != null)
                {
                    var value = propertyInfo.GetValue(mediaItem);

                    if (value != null)
                    {
                        if (value.GetType() == typeof (HttpPostedFileWrapper))
                        {
                            media.SetValue(p.Alias, value);
                        }
                        else
                        {
                            var s = propertyInfo.GetValue(mediaItem, null);

                            if (s != null)
                                media.SetValue(p.Alias, s.ToString());
                        }
                    }
                }
            }

            ApplicationContext.Current.Services.MediaService.Save(media);

            return media.Id;
        }

        #endregion
    }
}