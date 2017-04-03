﻿#pragma warning disable 0472
using System;
using System.Text;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Xml.Serialization;
using BLToolkit.Aspects;
using BLToolkit.DataAccess;
using BLToolkit.EditableObjects;
using BLToolkit.Data;
using BLToolkit.Data.DataProvider;
using BLToolkit.Mapping;
using BLToolkit.Reflection;
using bv.common.Configuration;
using bv.common.Enums;
using bv.common.Core;
using bv.model.BLToolkit;
using bv.model.Model;
using bv.model.Helpers;
using bv.model.Model.Extenders;
using bv.model.Model.Core;
using bv.model.Model.Handlers;
using bv.model.Model.Validators;
using eidss.model.Core;
using eidss.model.Enums;
		

namespace eidss.model.Schema
{
        
        
    [XmlType(AnonymousType = true)]
    public abstract partial class DataAudit : 
        EditableObject<DataAudit>
        , IObject
        , IDisposable
        , ILookupUsage
        {
        
        [MapField(_str_idfDataAuditEvent), NonUpdatable, PrimaryKey]
        public abstract Int64 idfDataAuditEvent { get; set; }
                
        [LocalizedDisplayName(_str_strObjectType)]
        [MapField(_str_strObjectType)]
        public abstract String strObjectType { get; set; }
        #if MONO
        protected String strObjectType_Original { get { return strObjectType; } }
        protected String strObjectType_Previous { get { return strObjectType; } }
        #else
        protected String strObjectType_Original { get { return ((EditableValue<String>)((dynamic)this)._strObjectType).OriginalValue; } }
        protected String strObjectType_Previous { get { return ((EditableValue<String>)((dynamic)this)._strObjectType).PreviousValue; } }
        #endif
                
        [LocalizedDisplayName(_str_idfsDataAuditEventType)]
        [MapField(_str_idfsDataAuditEventType)]
        public abstract Int64? idfsDataAuditEventType { get; set; }
        #if MONO
        protected Int64? idfsDataAuditEventType_Original { get { return idfsDataAuditEventType; } }
        protected Int64? idfsDataAuditEventType_Previous { get { return idfsDataAuditEventType; } }
        #else
        protected Int64? idfsDataAuditEventType_Original { get { return ((EditableValue<Int64?>)((dynamic)this)._idfsDataAuditEventType).OriginalValue; } }
        protected Int64? idfsDataAuditEventType_Previous { get { return ((EditableValue<Int64?>)((dynamic)this)._idfsDataAuditEventType).PreviousValue; } }
        #endif
                
        [LocalizedDisplayName(_str_strActionName)]
        [MapField(_str_strActionName)]
        public abstract String strActionName { get; set; }
        #if MONO
        protected String strActionName_Original { get { return strActionName; } }
        protected String strActionName_Previous { get { return strActionName; } }
        #else
        protected String strActionName_Original { get { return ((EditableValue<String>)((dynamic)this)._strActionName).OriginalValue; } }
        protected String strActionName_Previous { get { return ((EditableValue<String>)((dynamic)this)._strActionName).PreviousValue; } }
        #endif
                
        [LocalizedDisplayName(_str_idfMainObject)]
        [MapField(_str_idfMainObject)]
        public abstract Int64? idfMainObject { get; set; }
        #if MONO
        protected Int64? idfMainObject_Original { get { return idfMainObject; } }
        protected Int64? idfMainObject_Previous { get { return idfMainObject; } }
        #else
        protected Int64? idfMainObject_Original { get { return ((EditableValue<Int64?>)((dynamic)this)._idfMainObject).OriginalValue; } }
        protected Int64? idfMainObject_Previous { get { return ((EditableValue<Int64?>)((dynamic)this)._idfMainObject).PreviousValue; } }
        #endif
                
        [LocalizedDisplayName(_str_strTableName)]
        [MapField(_str_strTableName)]
        public abstract String strTableName { get; set; }
        #if MONO
        protected String strTableName_Original { get { return strTableName; } }
        protected String strTableName_Previous { get { return strTableName; } }
        #else
        protected String strTableName_Original { get { return ((EditableValue<String>)((dynamic)this)._strTableName).OriginalValue; } }
        protected String strTableName_Previous { get { return ((EditableValue<String>)((dynamic)this)._strTableName).PreviousValue; } }
        #endif
                
        [LocalizedDisplayName(_str_strSiteID)]
        [MapField(_str_strSiteID)]
        public abstract String strSiteID { get; set; }
        #if MONO
        protected String strSiteID_Original { get { return strSiteID; } }
        protected String strSiteID_Previous { get { return strSiteID; } }
        #else
        protected String strSiteID_Original { get { return ((EditableValue<String>)((dynamic)this)._strSiteID).OriginalValue; } }
        protected String strSiteID_Previous { get { return ((EditableValue<String>)((dynamic)this)._strSiteID).PreviousValue; } }
        #endif
                
        [LocalizedDisplayName(_str_datEnteringDate)]
        [MapField(_str_datEnteringDate)]
        public abstract DateTime? datEnteringDate { get; set; }
        #if MONO
        protected DateTime? datEnteringDate_Original { get { return datEnteringDate; } }
        protected DateTime? datEnteringDate_Previous { get { return datEnteringDate; } }
        #else
        protected DateTime? datEnteringDate_Original { get { return ((EditableValue<DateTime?>)((dynamic)this)._datEnteringDate).OriginalValue; } }
        protected DateTime? datEnteringDate_Previous { get { return ((EditableValue<DateTime?>)((dynamic)this)._datEnteringDate).PreviousValue; } }
        #endif
                
        [LocalizedDisplayName(_str_strHostname)]
        [MapField(_str_strHostname)]
        public abstract String strHostname { get; set; }
        #if MONO
        protected String strHostname_Original { get { return strHostname; } }
        protected String strHostname_Previous { get { return strHostname; } }
        #else
        protected String strHostname_Original { get { return ((EditableValue<String>)((dynamic)this)._strHostname).OriginalValue; } }
        protected String strHostname_Previous { get { return ((EditableValue<String>)((dynamic)this)._strHostname).PreviousValue; } }
        #endif
                
        [LocalizedDisplayName(_str_strPersonName)]
        [MapField(_str_strPersonName)]
        public abstract String strPersonName { get; set; }
        #if MONO
        protected String strPersonName_Original { get { return strPersonName; } }
        protected String strPersonName_Previous { get { return strPersonName; } }
        #else
        protected String strPersonName_Original { get { return ((EditableValue<String>)((dynamic)this)._strPersonName).OriginalValue; } }
        protected String strPersonName_Previous { get { return ((EditableValue<String>)((dynamic)this)._strPersonName).PreviousValue; } }
        #endif
                
        #region Set/Get values
        #region filed_info definifion
        protected class field_info
        {
            internal string _name;
            internal string _type;
            internal Func<DataAudit, object> _get_func;
            internal Action<DataAudit, string> _set_func;
            internal Action<DataAudit, DataAudit, CompareModel> _compare_func;
        }
        internal const string _str_Parent = "Parent";
        internal const string _str_IsNew = "IsNew";
        
        internal const string _str_idfDataAuditEvent = "idfDataAuditEvent";
        internal const string _str_strObjectType = "strObjectType";
        internal const string _str_idfsDataAuditEventType = "idfsDataAuditEventType";
        internal const string _str_strActionName = "strActionName";
        internal const string _str_idfMainObject = "idfMainObject";
        internal const string _str_strTableName = "strTableName";
        internal const string _str_strSiteID = "strSiteID";
        internal const string _str_datEnteringDate = "datEnteringDate";
        internal const string _str_strHostname = "strHostname";
        internal const string _str_strPersonName = "strPersonName";
        private static readonly field_info[] _field_infos =
        {
        
            new field_info {
              _name = _str_idfDataAuditEvent, _type = "Int64",
              _get_func = o => o.idfDataAuditEvent,
              _set_func = (o, val) => { o.idfDataAuditEvent = ParsingHelper.ParseInt64(val); },
              _compare_func = (o, c, m) => {
                if (o.idfDataAuditEvent != c.idfDataAuditEvent || o.IsRIRPropChanged(_str_idfDataAuditEvent, c)) 
                  m.Add(_str_idfDataAuditEvent, o.ObjectIdent + _str_idfDataAuditEvent, "Int64", o.idfDataAuditEvent == null ? "" : o.idfDataAuditEvent.ToString(), o.IsReadOnly(_str_idfDataAuditEvent), o.IsInvisible(_str_idfDataAuditEvent), o.IsRequired(_str_idfDataAuditEvent)); }
              }, 
        
            new field_info {
              _name = _str_strObjectType, _type = "String",
              _get_func = o => o.strObjectType,
              _set_func = (o, val) => { o.strObjectType = ParsingHelper.ParseString(val); },
              _compare_func = (o, c, m) => {
                if (o.strObjectType != c.strObjectType || o.IsRIRPropChanged(_str_strObjectType, c)) 
                  m.Add(_str_strObjectType, o.ObjectIdent + _str_strObjectType, "String", o.strObjectType == null ? "" : o.strObjectType.ToString(), o.IsReadOnly(_str_strObjectType), o.IsInvisible(_str_strObjectType), o.IsRequired(_str_strObjectType)); }
              }, 
        
            new field_info {
              _name = _str_idfsDataAuditEventType, _type = "Int64?",
              _get_func = o => o.idfsDataAuditEventType,
              _set_func = (o, val) => { o.idfsDataAuditEventType = ParsingHelper.ParseInt64Nullable(val); },
              _compare_func = (o, c, m) => {
                if (o.idfsDataAuditEventType != c.idfsDataAuditEventType || o.IsRIRPropChanged(_str_idfsDataAuditEventType, c)) 
                  m.Add(_str_idfsDataAuditEventType, o.ObjectIdent + _str_idfsDataAuditEventType, "Int64?", o.idfsDataAuditEventType == null ? "" : o.idfsDataAuditEventType.ToString(), o.IsReadOnly(_str_idfsDataAuditEventType), o.IsInvisible(_str_idfsDataAuditEventType), o.IsRequired(_str_idfsDataAuditEventType)); }
              }, 
        
            new field_info {
              _name = _str_strActionName, _type = "String",
              _get_func = o => o.strActionName,
              _set_func = (o, val) => { o.strActionName = ParsingHelper.ParseString(val); },
              _compare_func = (o, c, m) => {
                if (o.strActionName != c.strActionName || o.IsRIRPropChanged(_str_strActionName, c)) 
                  m.Add(_str_strActionName, o.ObjectIdent + _str_strActionName, "String", o.strActionName == null ? "" : o.strActionName.ToString(), o.IsReadOnly(_str_strActionName), o.IsInvisible(_str_strActionName), o.IsRequired(_str_strActionName)); }
              }, 
        
            new field_info {
              _name = _str_idfMainObject, _type = "Int64?",
              _get_func = o => o.idfMainObject,
              _set_func = (o, val) => { o.idfMainObject = ParsingHelper.ParseInt64Nullable(val); },
              _compare_func = (o, c, m) => {
                if (o.idfMainObject != c.idfMainObject || o.IsRIRPropChanged(_str_idfMainObject, c)) 
                  m.Add(_str_idfMainObject, o.ObjectIdent + _str_idfMainObject, "Int64?", o.idfMainObject == null ? "" : o.idfMainObject.ToString(), o.IsReadOnly(_str_idfMainObject), o.IsInvisible(_str_idfMainObject), o.IsRequired(_str_idfMainObject)); }
              }, 
        
            new field_info {
              _name = _str_strTableName, _type = "String",
              _get_func = o => o.strTableName,
              _set_func = (o, val) => { o.strTableName = ParsingHelper.ParseString(val); },
              _compare_func = (o, c, m) => {
                if (o.strTableName != c.strTableName || o.IsRIRPropChanged(_str_strTableName, c)) 
                  m.Add(_str_strTableName, o.ObjectIdent + _str_strTableName, "String", o.strTableName == null ? "" : o.strTableName.ToString(), o.IsReadOnly(_str_strTableName), o.IsInvisible(_str_strTableName), o.IsRequired(_str_strTableName)); }
              }, 
        
            new field_info {
              _name = _str_strSiteID, _type = "String",
              _get_func = o => o.strSiteID,
              _set_func = (o, val) => { o.strSiteID = ParsingHelper.ParseString(val); },
              _compare_func = (o, c, m) => {
                if (o.strSiteID != c.strSiteID || o.IsRIRPropChanged(_str_strSiteID, c)) 
                  m.Add(_str_strSiteID, o.ObjectIdent + _str_strSiteID, "String", o.strSiteID == null ? "" : o.strSiteID.ToString(), o.IsReadOnly(_str_strSiteID), o.IsInvisible(_str_strSiteID), o.IsRequired(_str_strSiteID)); }
              }, 
        
            new field_info {
              _name = _str_datEnteringDate, _type = "DateTime?",
              _get_func = o => o.datEnteringDate,
              _set_func = (o, val) => { o.datEnteringDate = ParsingHelper.ParseDateTimeNullable(val); },
              _compare_func = (o, c, m) => {
                if (o.datEnteringDate != c.datEnteringDate || o.IsRIRPropChanged(_str_datEnteringDate, c)) 
                  m.Add(_str_datEnteringDate, o.ObjectIdent + _str_datEnteringDate, "DateTime?", o.datEnteringDate == null ? "" : o.datEnteringDate.ToString(), o.IsReadOnly(_str_datEnteringDate), o.IsInvisible(_str_datEnteringDate), o.IsRequired(_str_datEnteringDate)); }
              }, 
        
            new field_info {
              _name = _str_strHostname, _type = "String",
              _get_func = o => o.strHostname,
              _set_func = (o, val) => { o.strHostname = ParsingHelper.ParseString(val); },
              _compare_func = (o, c, m) => {
                if (o.strHostname != c.strHostname || o.IsRIRPropChanged(_str_strHostname, c)) 
                  m.Add(_str_strHostname, o.ObjectIdent + _str_strHostname, "String", o.strHostname == null ? "" : o.strHostname.ToString(), o.IsReadOnly(_str_strHostname), o.IsInvisible(_str_strHostname), o.IsRequired(_str_strHostname)); }
              }, 
        
            new field_info {
              _name = _str_strPersonName, _type = "String",
              _get_func = o => o.strPersonName,
              _set_func = (o, val) => { o.strPersonName = ParsingHelper.ParseString(val); },
              _compare_func = (o, c, m) => {
                if (o.strPersonName != c.strPersonName || o.IsRIRPropChanged(_str_strPersonName, c)) 
                  m.Add(_str_strPersonName, o.ObjectIdent + _str_strPersonName, "String", o.strPersonName == null ? "" : o.strPersonName.ToString(), o.IsReadOnly(_str_strPersonName), o.IsInvisible(_str_strPersonName), o.IsRequired(_str_strPersonName)); }
              }, 
        
            new field_info()
        };
        #endregion
        
        private string _getType(string name)
        {
            var i = _field_infos.FirstOrDefault(n => n._name == name);
            return i == null ? "" : i._type;
        }
        private object _getValue(string name)
        {
            var i = _field_infos.FirstOrDefault(n => n._name == name);
            return i == null ? null : i._get_func(this);
        }
        private void _setValue(string name, string val)
        {
            var i = _field_infos.FirstOrDefault(n => n._name == name);
            if (i != null) i._set_func(this, val);
        }
        internal CompareModel _compare(IObject o, CompareModel ret)
        {
            if (ret == null) ret = new CompareModel();
            if (o == null) return ret;
            DataAudit obj = (DataAudit)o;
            foreach (var i in _field_infos)
                if (i != null && i._compare_func != null) i._compare_func(this, obj, ret);
            return ret;
        }
        #endregion
    
        private BvSelectList _getList(string name)
        {
        
            return null;
        }
    
        protected CacheScope m_CS;
        protected Accessor _getAccessor() { return Accessor.Instance(m_CS); }
        private IObjectPermissions m_permissions = null;
        internal IObjectPermissions _permissions { get { return m_permissions; } set { m_permissions = value; } }
        internal string m_ObjectName = "DataAudit";

        #region Parent and Clone supporting
        [XmlIgnore]
        public IObject Parent
        {
            get { return m_Parent; }
            set { m_Parent = value; /*OnPropertyChanged(_str_Parent);*/ }
        }
        private IObject m_Parent;
        internal void _setParent()
        {
        
        }
        public override object Clone()
        {
            var ret = base.Clone() as DataAudit;
            ret.m_Parent = this.Parent;
            ret._setParent();
            if (this.IsDirty && !ret.IsDirty)
                ret.SetChange();
            else if (!this.IsDirty && ret.IsDirty)
                ret.RejectChanges();
            return ret;
        }
        public IObject CloneWithSetup(DbManagerProxy manager)
        {
            var ret = base.Clone() as DataAudit;
            ret.m_Parent = this.Parent;
            ret.m_IsNew = this.IsNew;
            ret.m_ObjectName = this.m_ObjectName;
        
            Accessor acc = Accessor.Instance(null);
            acc._SetupLoad(manager, ret);
            ret._setParent();
            ret._permissions = _permissions;
            return ret;
        }
        public DataAudit CloneWithSetup()
        {
            using (DbManagerProxy manager = DbManagerFactory.Factory.Create(ModelUserContext.Instance))
            {
                return CloneWithSetup(manager) as DataAudit;
            }
        }
        #endregion

        #region IObject implementation
        public object Key { get { return idfDataAuditEvent; } }
        public string KeyName { get { return "idfDataAuditEvent"; } }
        public string ToStringProp { get { return ToString(); } }
        private bool m_IsNew;
        public bool IsNew { get { return m_IsNew; } }
        public bool HasChanges 
        { 
            get 
            { 
                return IsDirty
        
                ;
            }
        }
        public new void RejectChanges()
        {
        
            base.RejectChanges();
        
        }
        public void DeepRejectChanges()
        {
            RejectChanges();
        
        }
        public void DeepAcceptChanges()
        { 
            AcceptChanges();
        
        }
        private bool m_bForceDirty;
        public override void AcceptChanges()
        {
            m_bForceDirty = false;
            base.AcceptChanges();
        }
        public override bool IsDirty
        {
            get { return m_bForceDirty || base.IsDirty; }
        }
        public void SetChange()
        { 
            m_bForceDirty = true;
        }
        public void DeepSetChange()
        { 
            SetChange();
        
        }
        public bool MarkToDelete() { return _Delete(false); }
        public string ObjectName { get { return m_ObjectName; } }
        public string ObjectIdent { get { return ObjectName + "_" + Key.ToString() + "_"; } }
      public IObjectAccessor GetAccessor() { return _getAccessor(); }
      public IObjectPermissions GetPermissions() { return _permissions; }
      public bool ReadOnly { get { return _readOnly; } set { _readOnly = value; } }
      public bool IsReadOnly(string name) { return _isReadOnly(name); }
      public bool IsInvisible(string name) { return _isInvisible(name); }
      public bool IsRequired(string name) { return _isRequired(name); }
      public bool IsHiddenPersonalData(string name) { return _isHiddenPersonalData(name); }
      public string GetType(string name) { return _getType(name); }
      public object GetValue(string name) { return _getValue(name); }
      public void SetValue(string name, string val) { _setValue(name, val); }
      public CompareModel Compare(IObject o) { return _compare(o, null); }
      public BvSelectList GetList(string name) { return _getList(name); }
      public event ValidationEvent Validation;
      public event ValidationEvent ValidationEnd;
      public event AfterPostEvent AfterPost;
      
        public Dictionary<string, string> GetFieldTags(string name)
        {
          return null;
        }
      #endregion

      private bool IsRIRPropChanged(string fld, DataAudit c)
        {
            return IsReadOnly(fld) != c.IsReadOnly(fld) || IsInvisible(fld) != c.IsInvisible(fld) || IsRequired(fld) != c.IsRequired(fld);
        }

      

        public DataAudit()
        {
            
        }

        partial void Changed(string fieldName);
        partial void Created(DbManagerProxy manager);
        partial void Loaded(DbManagerProxy manager);
        partial void Deleted();

        

        private bool m_IsForcedToDelete;
        public bool IsForcedToDelete { get { return m_IsForcedToDelete; } }

        private bool m_IsMarkedToDelete;
        public bool IsMarkedToDelete { get { return m_IsMarkedToDelete; } }

        public void _SetupMainHandler()
        {
            PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(DataAudit_PropertyChanged);
        }
        public void _RevokeMainHandler()
        {
            PropertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(DataAudit_PropertyChanged);
        }
        private void DataAudit_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            (sender as DataAudit).Changed(e.PropertyName);
            
        }
        
        public bool ForceToDelete() { return _Delete(true); }
        internal bool _Delete(bool isForceDelete)
        {
            if (!_ValidateOnDelete()) return false;
            _DeletingExtenders();
            m_IsMarkedToDelete = true;
            m_IsForcedToDelete = m_IsForcedToDelete ? m_IsForcedToDelete : isForceDelete;
            OnPropertyChanged("IsMarkedToDelete");
            _DeletedExtenders();
            Deleted();
            return true;
        }
        private bool _ValidateOnDelete(bool bReport = true)
        {
            
            return true;                
              
        }
        private void _DeletingExtenders()
        {
            DataAudit obj = this;
            
        }
        private void _DeletedExtenders()
        {
            DataAudit obj = this;
            
        }
        
        public bool OnValidation(string msgId, string fldName, string prpName, object[] pars, Type type, bool shouldAsk)
        {
            if (Validation != null)
            {
                var args = new ValidationEventArgs(msgId, fldName, prpName, pars, type, this, shouldAsk);
                Validation(this, args);
                return args.Continue;
            }
            return false;
        }
        public bool OnValidationEnd(string msgId, string fldName, string prpName, object[] pars, Type type, bool shouldAsk)
        {
            if (ValidationEnd != null)
            {
                var args = new ValidationEventArgs(msgId, fldName, prpName, pars, type, this, shouldAsk);
                ValidationEnd(this, args);
                return args.Continue;
            }
            return false;
        }

        public void OnAfterPost()
        {
            if (AfterPost != null)
            {
                AfterPost(this, EventArgs.Empty);
            }
        }

        public FormSize FormSize
        {
            get { return FormSize.Undefined; }
        }
    
        private bool _isInvisible(string name)
        {
            
            return false;
                
        }

    
        private bool _isReadOnly(string name)
        {
            
            return ReadOnly;
                
        }

        private bool m_readOnly;
        private bool _readOnly
        {
            get { return m_readOnly; }
            set
            {
                m_readOnly = value;
        
            }
        }


        public Dictionary<string, Func<DataAudit, bool>> m_isRequired;
        private bool _isRequired(string name)
        {
            if (m_isRequired != null && m_isRequired.ContainsKey(name))
                return m_isRequired[name](this);
            return false;
        }
        
        public void AddRequired(string name, Func<DataAudit, bool> func)
        {
            if (m_isRequired == null) 
                m_isRequired = new Dictionary<string, Func<DataAudit, bool>>();
            if (!m_isRequired.ContainsKey(name))
                m_isRequired.Add(name, func);
        }
    
    public Dictionary<string, Func<DataAudit, bool>> m_isHiddenPersonalData;
    private bool _isHiddenPersonalData(string name)
    {
    if (m_isHiddenPersonalData != null && m_isHiddenPersonalData.ContainsKey(name))
    return m_isHiddenPersonalData[name](this);
    return false;
    }

    public void AddHiddenPersonalData(string name, Func<DataAudit, bool> func)
    {
    if (m_isHiddenPersonalData == null)
    m_isHiddenPersonalData = new Dictionary<string, Func<DataAudit, bool>>();
    if (!m_isHiddenPersonalData.ContainsKey(name))
    m_isHiddenPersonalData.Add(name, func);
    }
  
        #region IDisposable Members
        private bool bIsDisposed;
        ~DataAudit()
        {
            Dispose();
        }
        public void Dispose()
        {
            if (!bIsDisposed) 
            {
                bIsDisposed = true;
                
            }
        }
        #endregion
    
        #region ILookupUsage Members
        public void ReloadLookupItem(DbManagerProxy manager, string lookup_object)
        {
            
        }
        #endregion
    
        public void ParseFormCollection(NameValueCollection form, bool bParseLookups = true, bool bParseRelations = true)
        {
            if (bParseLookups)
            {
                _field_infos.Where(i => i._type == "Lookup").ToList().ForEach(a => { if (form[ObjectIdent + a._name] != null) a._set_func(this, form[ObjectIdent + a._name]);} );
            }
            
            _field_infos.Where(i => i._type != "Lookup" && i._type != "Child" && i._type != "Relation" && i._type != null)
                .ToList().ForEach(a => { if (form.AllKeys.Contains(ObjectIdent + a._name)) a._set_func(this, form[ObjectIdent + a._name]);} );
      
            if (bParseRelations)
            {
        
            }
        }
    

        #region Accessor
        public abstract partial class Accessor
        : DataAccessor<DataAudit>
            , IObjectAccessor
            , IObjectMeta
            , IObjectValidator
            
            , IObjectCreator
            
            , IObjectSelectDetail
            , IObjectPost
            , IObjectDelete
                    
        {
            private delegate void on_action(DataAudit obj);
            private static Accessor g_Instance = CreateInstance<Accessor>();
            private CacheScope m_CS;
            public static Accessor Instance(CacheScope cs) 
            { 
                if (cs == null)
                    return g_Instance;
                lock(cs)
                {
                    object acc = cs.Get(typeof (Accessor));
                    if (acc != null)
                    {
                        return acc as Accessor;
                    }
                    Accessor ret = CreateInstance<Accessor>();
                    ret.m_CS = cs;
                    cs.Add(typeof(Accessor), ret);
                    return ret;
                }
            }
            

            public virtual IObject SelectDetail(DbManagerProxy manager, object ident, int? HACode = null)
            {
                if (ident == null)
                {
                    return CreateNew(manager, null, HACode);
                }
                else
                {
                    return _SelectByKey(manager
                        , (Int64?)ident
                            
                        , null, null
                        );
                }
            }

            
            public virtual DataAudit SelectByKey(DbManagerProxy manager
                , Int64? idfDataAuditEvent
                )
            {
                return _SelectByKey(manager
                    , idfDataAuditEvent
                    , null, null
                    );
            }
            
      
            private DataAudit _SelectByKey(DbManagerProxy manager
                , Int64? idfDataAuditEvent
                , on_action loading, on_action loaded
                )
            {
            
                MapResultSet[] sets = new MapResultSet[1];
                List<DataAudit> objs = new List<DataAudit>();
                sets[0] = new MapResultSet(typeof(DataAudit), objs);
                
        
                try
                {
                    manager
                        .SetSpCommand("spDataAudit_SelectDetail"
                            , manager.Parameter("@idfDataAuditEvent", idfDataAuditEvent)
                            , manager.Parameter("@LangID", ModelUserContext.CurrentLanguage)
                            
                            )
                        .ExecuteResultSet(sets);

                    if (objs.Count == 0)
                        return null;

                    DataAudit obj = objs[0];
                    obj.m_CS = m_CS;
                    
                    if (loading != null)
                        loading(obj);
                    _SetupLoad(manager, obj);
                    
                    //obj._setParent();
                    if (loaded != null)
                        loaded(obj);
                    obj.Loaded(manager);
                    return obj;
                }
                catch(DataException e)
                {
                    throw DbModelException.Create(e);
                }
                
            }
    
        
        
            internal void _SetupLoad(DbManagerProxy manager, DataAudit obj)
            {
                if (obj == null) return;
                
                // loading extenters - begin
                // loading extenters - end
                
                _LoadLookups(manager, obj);
                obj._setParent();
                
                // loaded extenters - begin
                // loaded extenters - end
                
                _SetupHandlers(obj);
                _SetupChildHandlers(obj, null);
                
                _SetPermitions(obj._permissions, obj);
                _SetupRequired(obj);
                _SetupPersonalDataRestrictions(obj);
                obj._SetupMainHandler();
                obj.AcceptChanges();
            }
            
            internal void _SetPermitions(IObjectPermissions permissions, DataAudit obj)
            {
                if (obj != null)
                {
                    obj._permissions = permissions;
                    if (obj._permissions != null)
                    {
                    
                    }
                }
            }

    

            private DataAudit _CreateNew(DbManagerProxy manager, IObject Parent, int? HACode, on_action creating, on_action created)
            {
                try
                {
                    DataAudit obj = DataAudit.CreateInstance();
                    obj.m_CS = m_CS;
                    obj.m_IsNew = true;
                    obj.Parent = Parent;
                    
                    if (creating != null)
                        creating(obj);
                
                    // creating extenters - begin
                    // creating extenters - end
                
                    _LoadLookups(manager, obj);
                    _SetupHandlers(obj);
                    _SetupChildHandlers(obj, null);
                    
                    obj._SetupMainHandler();
                    obj._setParent();
                
                    // created extenters - begin
                    // created extenters - end
        
                    if (created != null)
                        created(obj);
                    obj.Created(manager);
                    _SetPermitions(obj._permissions, obj);
                    _SetupRequired(obj);
                    _SetupPersonalDataRestrictions(obj);
                    return obj;
                }
                catch(DataException e)
                {
                    throw DbModelException.Create(e);
                }
            }

            
            public DataAudit CreateNewT(DbManagerProxy manager, IObject Parent, int? HACode = null)
            {
                return _CreateNew(manager, Parent, HACode, null, null);
            }
            public IObject CreateNew(DbManagerProxy manager, IObject Parent, int? HACode = null)
            {
                return _CreateNew(manager, Parent, HACode, null, null);
            }
            
            public DataAudit CreateWithParamsT(DbManagerProxy manager, IObject Parent, List<object> pars)
            {
                return _CreateNew(manager, Parent, null, null, null);
            }
            public IObject CreateWithParams(DbManagerProxy manager, IObject Parent, List<object> pars)
            {
                return _CreateNew(manager, Parent, null, null, null);
            }
            
            private void _SetupChildHandlers(DataAudit obj, object newobj)
            {
                
            }
            
            private void _SetupHandlers(DataAudit obj)
            {
                
            }
    

            private void _LoadLookups(DbManagerProxy manager, DataAudit obj)
            {
                
            }
    
            public bool DeleteObject(DbManagerProxy manager, object ident)
            {
                IObject obj = SelectDetail(manager, ident);
                if (!obj.MarkToDelete()) return false;
                return Post(manager, obj);
            }
        
            public bool Post(DbManagerProxy manager, IObject obj, bool bChildObject = false) 
            {
                throw new NotImplementedException();
            }
            
        
            public bool Validate(DbManagerProxy manager, IObject obj, bool bPostValidation, bool bChangeValidation, bool bDeepValidation, bool bRethrowException = false)
            {
                return Validate(manager, obj as DataAudit, bPostValidation, bChangeValidation, bDeepValidation, bRethrowException);
            }
            public bool Validate(DbManagerProxy manager, DataAudit obj, bool bPostValidation, bool bChangeValidation, bool bDeepValidation, bool bRethrowException = false)
            {
                
                return true;
            }
           
    
            private void _SetupRequired(DataAudit obj)
            {
            
            }
    
    private void _SetupPersonalDataRestrictions(DataAudit obj)
    {
    
    
    }
  
            #region IObjectMeta
            public int? MaxSize(string name) { return Meta.Sizes.ContainsKey(name) ? (int?)Meta.Sizes[name] : null; }
            public bool RequiredByField(string name, IObject obj) { return Meta.RequiredByField.ContainsKey(name) ? Meta.RequiredByField[name](obj as DataAudit) : false; }
            public bool RequiredByProperty(string name, IObject obj) { return Meta.RequiredByProperty.ContainsKey(name) ? Meta.RequiredByProperty[name](obj as DataAudit) : false; }
            public List<SearchPanelMetaItem> SearchPanelMeta { get { return Meta.SearchPanelMeta; } }
            public List<GridMetaItem> GridMeta { get { return Meta.GridMeta; } }
            public List<ActionMetaItem> Actions { get { return Meta.Actions; } }
            public string DetailPanel { get { return "DataAuditDetail"; } }
            public string HelpIdWin { get { return ""; } }
            public string HelpIdWeb { get { return ""; } }
            public string HelpIdHh { get { return ""; } }
            #endregion
    
        }

        
        #region Meta
        public static class Meta
        {
            public static string spSelect = "spDataAudit_SelectDetail";
            public static string spCount = "";
            public static string spPost = "";
            public static string spInsert = "";
            public static string spUpdate = "";
            public static string spDelete = "";
            public static string spCanDelete = "";
        
            public static Dictionary<string, int> Sizes = new Dictionary<string, int>();
            public static Dictionary<string, Func<DataAudit, bool>> RequiredByField = new Dictionary<string, Func<DataAudit, bool>>();
            public static Dictionary<string, Func<DataAudit, bool>> RequiredByProperty = new Dictionary<string, Func<DataAudit, bool>>();
            public static List<SearchPanelMetaItem> SearchPanelMeta = new List<SearchPanelMetaItem>();
            public static List<GridMetaItem> GridMeta = new List<GridMetaItem>();
            public static List<ActionMetaItem> Actions = new List<ActionMetaItem>();
            static Meta()
            {
                
                Sizes.Add(_str_strObjectType, 2000);
                Sizes.Add(_str_strActionName, 2000);
                Sizes.Add(_str_strTableName, 200);
                Sizes.Add(_str_strSiteID, 36);
                Sizes.Add(_str_strHostname, 200);
                Sizes.Add(_str_strPersonName, 602);
                Actions.Add(new ActionMetaItem(
                    "Create",
                    ActionTypes.Create,
                    false,
                    String.Empty,
                    String.Empty,
                    (manager, c, pars) => new ActResult(true, Accessor.Instance(null).CreateWithParams(manager, null, pars)),
                    null,
                    new ActionMetaItem.VisualItem(
                        /*from BvMessages*/"strCreate_Id",
                        "add",
                        /*from BvMessages*/"tooltipCreate_Id",
                        /*from BvMessages*/"",
                        "",
                        /*from BvMessages*/"tooltipCreate_Id",
                        ActionsAlignment.Right,
                        ActionsPanelType.Main,
                        ActionsAppType.All
                      ),
                      false,
                      null,
                      null,
                      null,
                      null,
                      null,
                      false
                      ));
                    
                Actions.Add(new ActionMetaItem(
                    "Delete",
                    ActionTypes.Delete,
                    false,
                    String.Empty,
                    String.Empty,
                    (manager, c, pars) => new ActResult(((DataAudit)c).MarkToDelete() && ObjectAccessor.PostInterface<DataAudit>().Post(manager, (DataAudit)c), c),
                    null,
                    new ActionMetaItem.VisualItem(
                        /*from BvMessages*/"strDelete_Id",
                        "Delete_Remove",
                        /*from BvMessages*/"tooltipDelete_Id",
                        /*from BvMessages*/"",
                        "",
                        /*from BvMessages*/"tooltipDelete_Id",
                        ActionsAlignment.Right,
                        ActionsPanelType.Main,
                        ActionsAppType.All
                      ),
                      false,
                      null,
                      null,
                      (o, p, r) => r && !o.IsNew && !o.HasChanges,
                      null,
                      null,
                      false
                      ));
                    
                Actions.Add(new ActionMetaItem(
                    "Save",
                    ActionTypes.Save,
                    false,
                    String.Empty,
                    String.Empty,
                    (manager, c, pars) => new ActResult(ObjectAccessor.PostInterface<DataAudit>().Post(manager, (DataAudit)c), c),
                    null,
                    new ActionMetaItem.VisualItem(
                        /*from BvMessages*/"strSave_Id",
                        "Save",
                        /*from BvMessages*/"tooltipSave_Id",
                        /*from BvMessages*/"",
                        "",
                        /*from BvMessages*/"tooltipSave_Id",
                        ActionsAlignment.Right,
                        ActionsPanelType.Main,
                        ActionsAppType.All
                      ),
                      false,
                      null,
                      null,
                      null,
                      null,
                      null,
                      false
                      ));
                    
                Actions.Add(new ActionMetaItem(
                    "Ok",
                    ActionTypes.Ok,
                    false,
                    String.Empty,
                    String.Empty,
                    (manager, c, pars) => new ActResult(ObjectAccessor.PostInterface<DataAudit>().Post(manager, (DataAudit)c), c),
                    null,
                    new ActionMetaItem.VisualItem(
                        /*from BvMessages*/"strOK_Id",
                        "",
                        /*from BvMessages*/"tooltipOK_Id",
                        /*from BvMessages*/"",
                        "",
                        /*from BvMessages*/"tooltipOK_Id",
                        ActionsAlignment.Right,
                        ActionsPanelType.Main,
                        ActionsAppType.All
                      ),
                      false,
                      null,
                      null,
                      null,
                      null,
                      null,
                      false
                      ));
                    
                Actions.Add(new ActionMetaItem(
                    "Cancel",
                    ActionTypes.Cancel,
                    false,
                    String.Empty,
                    String.Empty,
                    (manager, c, pars) => new ActResult(true, c),
                    null,
                    new ActionMetaItem.VisualItem(
                        /*from BvMessages*/"strCancel_Id",
                        "",
                        /*from BvMessages*/"tooltipCancel_Id",
                        /*from BvMessages*/"strOK_Id",
                        "",
                        /*from BvMessages*/"tooltipCancel_Id",
                        ActionsAlignment.Right,
                        ActionsPanelType.Main,
                        ActionsAppType.All
                      ),
                      false,
                      null,
                      null,
                      null,
                      null,
                      null,
                      false
                      ));
                    
        
            }
        }
        #endregion
    

        #endregion
        }
    
}
	