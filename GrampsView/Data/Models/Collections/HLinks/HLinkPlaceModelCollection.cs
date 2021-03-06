﻿// <copyright file="HLinkPlaceModelCollection.cs" company="PlaceholderCompany">
//     Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

/// <summary>
/// </summary>
namespace GrampsView.Data.Collections
{
    using GrampsView.Common;
    using GrampsView.Data.DataView;
    using GrampsView.Data.Model;

    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Runtime.Serialization;

    /// <summary>
    /// </summary>
    [CollectionDataContract]
    [KnownType(typeof(ObservableCollection<HLinkCitationModel>))]
    public class HLinkPlaceModelCollection : HLinkBaseCollection<HLinkPlaceModel>
    {
        public HLinkPlaceModelCollection()
        {
            Title = "Place Collection";
        }

        public override CardGroup GetCardGroup()
        {
            CardGroup t = base.GetCardGroup();

            t.Title = Title;

            return t;
        }

        /// <summary>
        /// Helper method to sort and set the firt image link.
        /// </summary>
        public void SortAndSetFirst()
        {
            // Set the first image link. Assumes main image is manually set to the first image in
            // Gramps if we need it to be, e.g. Citations.
            PlaceModel tempModel = new PlaceModel();

            FirstHLinkHomeImage.HomeImageType = CommonEnums.HomeImageType.Unknown;

            if (Count > 0)
            {
                // Step through each citationmodel hlink in the collection
                for (int i = 0; i < Count; i++)
                {
                    tempModel = DV.PlaceDV.PlaceData.GetModelFromHLink(this[i]);

                    if (tempModel.HomeImageHLink.LinkToImage)
                    {
                        FirstHLinkHomeImage = tempModel.HomeImageHLink;
                        break;
                    }
                }

                // Sort the collection
                List<HLinkPlaceModel> t = this.OrderBy(HLinkCitationModel => HLinkCitationModel.DeRef.GetDefaultText).ToList();

                Items.Clear();

                foreach (HLinkPlaceModel item in t)
                {
                    Items.Add(item);
                }
            }
        }
    }
}