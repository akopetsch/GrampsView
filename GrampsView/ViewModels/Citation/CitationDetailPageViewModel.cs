﻿/// <summary>
/// Defines the Citation Detail Page View Model
/// </summary>
namespace GrampsView.ViewModels
{
    using GrampsView.Common;
    using GrampsView.Data.DataView;
    using GrampsView.Data.Model;

    using Prism.Events;
    using Prism.Navigation;

    using System.Globalization;

    /// <summary>
    /// Defines the Citation Detail Page View ViewModel.
    /// </summary>
    public class CitationDetailViewModel : ViewModelBase
    {
        /// <summary>
        /// Holds the Note ViewModel.
        /// </summary>
        private CitationModel localCitationObject = new CitationModel();

        /// <summary>
        /// Initializes a new instance of the <see cref="CitationDetailViewModel"/> class.
        /// </summary>
        /// <param name="iocCommonLogging">
        /// </param>
        /// <param name="iocEventAggregator">
        /// The ioc event aggregator.
        /// </param>
        /// <param name="iocNavigationService">
        /// Navigation Service
        /// </param>
        public CitationDetailViewModel(ICommonLogging iocCommonLogging, IEventAggregator iocEventAggregator, INavigationService iocNavigationService)
            : base(iocCommonLogging, iocEventAggregator, iocNavigationService)
        {
        }

        /// <summary>
        /// Gets or sets the citation object.
        /// </summary>
        /// <value>
        /// The citation object.
        /// </value>

        public CitationModel CitationObject
        {
            get
            {
                return localCitationObject;
            }

            set
            {
                SetProperty(ref localCitationObject, value);
            }
        }

        /// <summary>
        /// Handles navigation in wards and sets up the event model parameter.
        /// </summary>
        /// <param name="e">
        /// The <see cref="NavigatedToEventArgs"/> instance containing the event data.
        /// </param>
        /// <param name="viewModelState">
        /// The parameter is not used.
        /// </param>
        public override void PopulateViewModel()
        {
            // Handle HLinkKeys
            CitationObject = DV.CitationDV.GetModelFromHLinkString(BaseNavParamsHLink.HLinkKey);

            // Trigger refresh of View fields via INotifyPropertyChanged
            RaisePropertyChanged(string.Empty);

            if (CitationObject != null)
            {
                BaseTitle = CitationObject.GetDefaultText;
                BaseTitleIcon = CommonConstants.IconCitation;

                // Get basic details
                //   CardGroup t = new CardGroup { Title = "Header Details" };

                BaseDetail.Add(new CardListLineCollection
                    {
                            new CardListLine("Card Type:", "Citation Detail"),
                            new CardListLine("Page:", CitationObject.GPage),
                            new CardListLine("Confidence:", CitationObject.GConfidence.ToString(CultureInfo.CurrentCulture)),
                    });

                // Get date card
                BaseDetail.Add(CitationObject.GDateContent.AsCardListLine());

                BaseDetail.Add(DV.CitationDV.GetModelInfoFormatted(CitationObject));

                //BaseDetail.Add(t);

                // If only one note (the most common case) just display it in a large format,
                // otherwise setup a list of them.
                if (CitationObject.GNoteRefCollection.Count > 0)
                {
                    // TODO Fix this NoteObject = CitationObject.GNoteRefCollection[0].DeRef;
                }

                // TODO BaseDetail.Add(CitationObject.GSourceAttributeCollection);
            }
        }
    }
}