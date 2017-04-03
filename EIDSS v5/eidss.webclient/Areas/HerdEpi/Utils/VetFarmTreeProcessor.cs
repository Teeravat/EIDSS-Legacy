﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eidss.webclient.Utils;
using BLToolkit.EditableObjects;
using eidss.model.Schema;
using bv.model.Model.Core;
using bv.model.BLToolkit;
using System.Web.Mvc;
using eidss.model.Resources;
using eidss.model.Enums;
using eidss.model.Core;
using System.Drawing;

namespace eidss.webclient.Areas.HerdEpi.Utils
{
    public class VetFarmTreeProcessor
    { 
        private const string KEY_FOR_TEMP_ITEM_STORAGE = "Species_Item";
        private static string m_error;

        private static bool CreateHerdOrFlock(string sessionId, long key, string name, bool blnLvstck, out string error)
        {
            try
            {
                var list = ModelStorage.Get(sessionId, key, name, true) as EditableList<VetFarmTree>;
                var rootobj = ModelStorage.GetRoot(sessionId, key, null) as IObject;
                error = string.Empty;

                using (DbManagerProxy manager = DbManagerFactory.Factory.Create(ModelUserContext.Instance))
                {
                    VetFarmTree root = list.Where(v => v.idfParentParty == null).FirstOrDefault();
                    var item = VetFarmTree.Accessor.Instance(null).CreateHerd(manager, rootobj, root);
                    list.Add(item);
                    list.Sort(System.ComponentModel.ListSortDirection.Ascending, new string[] {"strHerdName", "strSpeciesName"});
                    return true;
                }
            }
            catch (ObjectNotFoundException)
            {
                error = "ObjectExpired";
                return false;
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
        }



        public static bool CreateHerd(string sessionId, long key, string name, out string error)
        {
            return CreateHerdOrFlock(sessionId, key, name, true, out error);
        }

        public static bool CreateFlock(string sessionId, long key, string name, out string error)
        {
            return CreateHerdOrFlock(sessionId, key, name, false, out error);
        }


        public static bool UpdateSpecies(string sessionId, long key, string name, bool isNew, FormCollection form, out string error)
        {
            try
            {
                error = string.Empty; m_error = string.Empty;
                var spec = ModelStorage.Get(sessionId, key, KEY_FOR_TEMP_ITEM_STORAGE) as VetFarmTree;
                var farm = (ModelStorage.Get(sessionId, key, "") as VetCase).Farm;
                var list = farm.FarmTree;

                spec.Validation += object_ValidationDetails;
                spec.ParseFormCollection(form);                
                using (DbManagerProxy manager = DbManagerFactory.Factory.Create(EidssUserContext.Instance))
                {
                    VetFarmTree.Accessor.Instance(null).Validate(manager, spec, true, false, false);
                }

                spec.Validation -= object_ValidationDetails;                
                if (String.IsNullOrWhiteSpace(m_error) && isNew)
                {
                    farm.Validation += object_ValidationDetails;
                    list.Add(spec);
                    farm.Validation -= object_ValidationDetails;
                }                               

                if (!String.IsNullOrWhiteSpace(m_error))
                {
                    error = m_error;
                    return false;
                }

                var herd = list.Where(x => x.idfParty == spec.idfParentParty).FirstOrDefault();
                spec.strHerdName = herd.strName;

                list.Sort(System.ComponentModel.ListSortDirection.Ascending, new string[] { "strHerdName", "strSpeciesName" });

                return true;
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
           
        }

        public static VetFarmTree GetSpecies(string sessionId, long key, string name,long? idfParty, long? idfSpecies)
        {
            var vetcase = ModelStorage.Get(sessionId, key, null) as VetCase;
            if (vetcase == null)
                return null;

            var list = vetcase.Farm.FarmTree;//ModelStorage.Get(sessionId, key, name) as EditableList<VetFarmTree>;            

            VetFarmTree item = null;
            VetFarmTree farmTree = null;
            
            using (DbManagerProxy manager = DbManagerFactory.Factory.Create(ModelUserContext.Instance))
            {
                if ((idfParty.HasValue && idfParty.Value > 0))
                {
                    farmTree = list.Single(t => t.idfParty == idfParty);
                    switch (farmTree.idfsPartyType)
                    {
                        case (int)PartyTypeEnum.Herd :
                            item = VetFarmTree.Accessor.Instance(null).CreateSpeciesWithDiagnosis(manager, vetcase, farmTree, vetcase.idfsDiagnosis);
                            break;
                        case (int)PartyTypeEnum.Species :
                            item = VetFarmTree.Accessor.Instance(null).CreateSpeciesWithDiagnosis(manager, vetcase, list.Where(x => x.idfParty == farmTree.idfParentParty).First(), vetcase.idfsDiagnosis);
                            break;
                        default :
                            item = VetFarmTree.Accessor.Instance(null).CreateSpeciesWithDiagnosis(manager, vetcase, list.Where(x => x.idfsPartyType == (int)PartyTypeEnum.Herd && !x.IsMarkedToDelete).FirstOrDefault(), vetcase.idfsDiagnosis);
                            item.idfParentParty = null;
                            break;
                    }                                      
               }
                else
                {
                    if (idfSpecies.HasValue && idfSpecies.Value > 0)
                    {
                        item = list.Single(x => x.idfParty == idfSpecies);
                    }
                    else
                    {
                        item = VetFarmTree.Accessor.Instance(null).CreateSpeciesWithDiagnosis(manager, vetcase, list.Where(x => x.idfsPartyType == (int)PartyTypeEnum.Herd && !x.IsMarkedToDelete).FirstOrDefault(), vetcase.idfsDiagnosis);
                    }
                }                                                                    
                    
                ModelStorage.Put(sessionId, key, key, KEY_FOR_TEMP_ITEM_STORAGE, item);
                return item;
            }
        }

        static void object_ValidationDetails(object sender, ValidationEventArgs args)
        {
            m_error = EidssMessages.GetValidationErrorMessage(args);
        }

    }
}