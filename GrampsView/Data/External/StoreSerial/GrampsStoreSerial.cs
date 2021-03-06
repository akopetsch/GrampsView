﻿//-----------------------------------------------------------------------
//
// Repository Serialisation/Deserialisation code
//
// <copyright file="GrampsStoreSerial.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace GrampsView.Data.External.StoreSerial
{
    using GrampsView.Common;
    using GrampsView.Data.DataView;
    using GrampsView.Data.Repository;

    using Newtonsoft.Json;

    using System;
    using System.IO;
    using System.IO.IsolatedStorage;
    using System.Runtime.Serialization;
    using System.Threading.Tasks;

    /// <summary>
    /// Creates a collection of entities with content read from a GRAMPS XML file.
    /// </summary>
    public class GrampsStoreSerial : IGrampsStoreSerial
    {
        /// <summary>
        /// The local common logging.
        /// </summary>
        private readonly ICommonLogging localGVLogging;

        /// <summary>
        /// Initializes a new instance of the <see cref="GrampsStoreSerial"/> class.
        /// </summary>
        /// <param name="iocGVProgress">
        /// The ioc gv progress.
        /// </param>
        /// <param name="iocGVLogging">
        /// The ioc gv logging.
        /// </param>
        public GrampsStoreSerial(ICommonLogging iocGVLogging)
        {
            // save injected references for later
            localGVLogging = iocGVLogging;
        }

        /// <summary>
        /// Deserialise the previously serialised repository. Perform as a single step so that it
        /// goes faster at the cost of providing less feedbak to the user.
        /// </summary>

        public void DeSerializeRepository()
        {
            localGVLogging.LogRoutineEntry(nameof(DeSerializeRepository));

            try
            {
                DataContractSerializer ser = new DataContractSerializer(typeof(DataInstance));

                using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    // Check of the file exists
                    string DataInstanceFileName = typeof(DataInstance).Name.Trim() + ".json";

                    if (!isoStore.FileExists(DataInstanceFileName))
                    {
                        DataStore.CN.NotifyError("DeSerializeRepository - File: " + DataInstanceFileName + " does not exist.  Reload the GPKG file");
                        CommonLocalSettings.DataSerialised = false;
                        return;
                    }

                    var stream = new IsolatedStorageFileStream(DataInstanceFileName, FileMode.Open, isoStore);

                    using (StreamReader file = new StreamReader(stream))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Converters.Add(new GrampsView.Converters.NewtonSoftColorConverter());

                        DataInstance t = (DataInstance)serializer.Deserialize(file, typeof(DataInstance));

                        // Check for nulls
                        if (t.AddressData != null)
                        {
                            DataStore.DS.AddressData = t.AddressData;
                        }
                        else
                        {
                            CommonLocalSettings.DataSerialised = false;
                            DataStore.CN.NotifyError("Bad Address deserialisation error.  Data loading cancelled. Restart the program and reload the data.");
                        }

                        if (t.BookMarkCollection != null)
                        {
                            DataStore.DS.BookMarkCollection = t.BookMarkCollection;
                        }
                        else
                        {
                            CommonLocalSettings.DataSerialised = false;
                            DataStore.CN.NotifyError("Bad BookMark deserialisation error.  Data loading cancelled. Restart the program and reload the data.");
                        }

                        if (t.CitationData != null)
                        {
                            DataStore.DS.CitationData = t.CitationData;
                        }
                        else
                        {
                            CommonLocalSettings.DataSerialised = false;
                            DataStore.CN.NotifyError("Bad Citation deserialisation error.  Data loading cancelled. Restart the program and reload the data.");
                        }

                        if (t.EventData != null)
                        {
                            DataStore.DS.EventData = t.EventData;
                        }
                        else
                        {
                            CommonLocalSettings.DataSerialised = false;
                            DataStore.CN.NotifyError("Bad Event deserialisation error.  Data loading cancelled. Restart the program and reload the data.");
                        }

                        if (t.FamilyData != null)
                        {
                            DataStore.DS.FamilyData = t.FamilyData;
                        }
                        else
                        {
                            CommonLocalSettings.DataSerialised = false;
                            DataStore.CN.NotifyError("Bad Family deserialisation error.  Data loading cancelled. Restart the program and reload the data.");
                        }

                        if (t.MediaData != null)
                        {
                            DataStore.DS.MediaData = t.MediaData;
                        }
                        else
                        {
                            CommonLocalSettings.DataSerialised = false;
                            DataStore.CN.NotifyError("Bad Media deserialisation error.  Data loading cancelled. Restart the program and reload the data.");
                        }

                        if (t.PersonData != null)
                        {
                            DataStore.DS.PersonData = t.PersonData;
                        }
                        else
                        {
                            CommonLocalSettings.DataSerialised = false;
                            DataStore.CN.NotifyError("Bad Person deserialisation error.  Data loading cancelled. Restart the program and reload the data.");
                        }

                        if (t.PersonNameData != null)
                        {
                            DataStore.DS.PersonNameData = t.PersonNameData;
                        }
                        else
                        {
                            CommonLocalSettings.DataSerialised = false;
                            DataStore.CN.NotifyError("Bad Person Name deserialisation error.  Data loading cancelled. Restart the program and reload the data.");
                        }

                        // Check for nulls
                        if (t.SourceData != null)
                        {
                            DataStore.DS.SourceData = t.SourceData;
                        }
                        else
                        {
                            CommonLocalSettings.DataSerialised = false;
                            DataStore.CN.NotifyError("Bad Source data deserialisation error.  Data loading cancelled. Restart the program and reload the data.");
                        }

                        // Check for nulls
                        if (t.TagData != null)
                        {
                            DataStore.DS.TagData = t.TagData;
                        }
                        else
                        {
                            CommonLocalSettings.DataSerialised = false;
                            DataStore.CN.NotifyError("Bad Tag data deserialisation error.  Data loading cancelled. Restart the program and reload the data.");
                        }

                        // TODO Finish setting the checks up on these
                        DataStore.DS.HeaderData = t.HeaderData;
                        DataStore.DS.NameMapData = t.NameMapData;
                        DataStore.DS.NoteData = t.NoteData;

                        DataStore.DS.PlaceData = t.PlaceData;
                        DataStore.DS.RepositoryData = t.RepositoryData;
                    }
                }

                localGVLogging.LogRoutineExit(nameof(DeSerializeRepository));
            }
            catch (Exception ex)
            {
                localGVLogging.LogProgress("DeSerializeRepository - Exception ");
                CommonLocalSettings.DataSerialised = false;
                DataStore.CN.NotifyException("Old data deserialisation error.  Data loading cancelled", ex);
                throw;
            }

            return;
        }

        /// <summary>
        /// Serializes the object.
        /// </summary>
        /// <param name="theObject">
        /// The object.
        /// </param>
        /// <returns>
        /// </returns>
        public bool SerializeObject(object theObject)
        {
            try
            {
                JsonSerializer serializer = new JsonSerializer();

                serializer.Converters.Add(new GrampsView.Converters.NewtonSoftColorConverter());

                using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream(typeof(DataInstance).Name.Trim() + ".json", FileMode.Create, isoStore))
                    {
                        StreamWriter sw = new StreamWriter(stream);

                        using (JsonWriter writer = new JsonTextWriter(sw))
                        {
                            serializer.Serialize(writer, theObject);
                        }
                    }
                }

                CommonLocalSettings.DataSerialised = true;
                return true;
            }
            catch (Exception ex)
            {
                DataStore.CN.NotifyException("Trying to serialise object ", ex);
                CommonLocalSettings.DataSerialised = false;
                throw;
            }
        }
    }
}