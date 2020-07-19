﻿//-----------------------------------------------------------------------
//
// Handles GRAMPS Alt fields
//
// <copyright file="SrcAttributeModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using GrampsView.Data.Collections;
using GrampsView.Data.DataView;

namespace GrampsView.Data.Model
{
    /// <summary>
    /// GRAMPS Alt element class.
    /// </summary>
    public class PersonRefModel : ModelBase, IPersonRefModel
    {
        public new PersonModel DeRef
        {
            get
            {
                if (Valid)
                {
                    return DV.PersonDV.GetModelFromHLinkString(HLinkKey);
                }
                else
                {
                    return new PersonModel();
                }
            }
        }

        public HLinkCitationModelCollection GCitationCollection
        {
            get; set;
        }

            = new HLinkCitationModelCollection();

        /// <summary>
        /// Gets or sets the g text.
        /// </summary>
        /// <value>
        /// The g text.
        /// </value>
        public HLinkNoteModelCollection GNoteCollection
        {
            get; set;
        }

            = new HLinkNoteModelCollection();

        public string GRelationship { get; set; }
    }
}