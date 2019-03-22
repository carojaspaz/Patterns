﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Mockservice.AfisDijin {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://transaction.ws.adapter.edi.bodega.morphotrak.com/", ConfigurationName="AfisDijin.Transaction")]
    public interface Transaction {
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        Mockservice.AfisDijin.submitNistFileResponse submitNistFile(Mockservice.AfisDijin.submitNistFileRequest request);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="", ReplyAction="*")]
        System.IAsyncResult BeginsubmitNistFile(Mockservice.AfisDijin.submitNistFileRequest request, System.AsyncCallback callback, object asyncState);
        
        Mockservice.AfisDijin.submitNistFileResponse EndsubmitNistFile(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        Mockservice.AfisDijin.getNistFileResponseResponse getNistFileResponse(Mockservice.AfisDijin.getNistFileResponseRequest request);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="", ReplyAction="*")]
        System.IAsyncResult BegingetNistFileResponse(Mockservice.AfisDijin.getNistFileResponseRequest request, System.AsyncCallback callback, object asyncState);
        
        Mockservice.AfisDijin.getNistFileResponseResponse EndgetNistFileResponse(System.IAsyncResult result);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3056.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://transaction.ws.adapter.edi.bodega.morphotrak.com/")]
    public partial class RequestInfo : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string requestIdField;
        
        private string userIdField;
        
        private string clientIdField;
        
        private string stationField;
        
        private string dateTimeField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string requestId {
            get {
                return this.requestIdField;
            }
            set {
                this.requestIdField = value;
                this.RaisePropertyChanged("requestId");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string userId {
            get {
                return this.userIdField;
            }
            set {
                this.userIdField = value;
                this.RaisePropertyChanged("userId");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string clientId {
            get {
                return this.clientIdField;
            }
            set {
                this.clientIdField = value;
                this.RaisePropertyChanged("clientId");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public string station {
            get {
                return this.stationField;
            }
            set {
                this.stationField = value;
                this.RaisePropertyChanged("station");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=4)]
        public string dateTime {
            get {
                return this.dateTimeField;
            }
            set {
                this.dateTimeField = value;
                this.RaisePropertyChanged("dateTime");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3056.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://transaction.ws.adapter.edi.bodega.morphotrak.com/")]
    public partial class NistResponse : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string requestIdField;
        
        private string responseDateTimeField;
        
        private submissionState stateField;
        
        private bool stateFieldSpecified;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string requestId {
            get {
                return this.requestIdField;
            }
            set {
                this.requestIdField = value;
                this.RaisePropertyChanged("requestId");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string responseDateTime {
            get {
                return this.responseDateTimeField;
            }
            set {
                this.responseDateTimeField = value;
                this.RaisePropertyChanged("responseDateTime");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public submissionState state {
            get {
                return this.stateField;
            }
            set {
                this.stateField = value;
                this.RaisePropertyChanged("state");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool stateSpecified {
            get {
                return this.stateFieldSpecified;
            }
            set {
                this.stateFieldSpecified = value;
                this.RaisePropertyChanged("stateSpecified");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3056.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://transaction.ws.adapter.edi.bodega.morphotrak.com/")]
    public enum submissionState {
        
        /// <remarks/>
        SUBMITTED,
        
        /// <remarks/>
        RESPONSES_AVAILABLE,
        
        /// <remarks/>
        COMPLETED,
        
        /// <remarks/>
        ERROR,
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="submitNistFile", WrapperNamespace="http://transaction.ws.adapter.edi.bodega.morphotrak.com/", IsWrapped=true)]
    public partial class submitNistFileRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=0)]
        public Mockservice.AfisDijin.RequestInfo requestInfo;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=1)]
        [System.Xml.Serialization.XmlElementAttribute(DataType="hexBinary")]
        public byte[] nistFile;
        
        public submitNistFileRequest() {
        }
        
        public submitNistFileRequest(Mockservice.AfisDijin.RequestInfo requestInfo, byte[] nistFile) {
            this.requestInfo = requestInfo;
            this.nistFile = nistFile;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="submitNistFileResponse", WrapperNamespace="http://transaction.ws.adapter.edi.bodega.morphotrak.com/", IsWrapped=true)]
    public partial class submitNistFileResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=0)]
        public Mockservice.AfisDijin.submissionState state;
        
        public submitNistFileResponse() {
        }
        
        public submitNistFileResponse(Mockservice.AfisDijin.submissionState state) {
            this.state = state;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getNistFileResponse", WrapperNamespace="http://transaction.ws.adapter.edi.bodega.morphotrak.com/", IsWrapped=true)]
    public partial class getNistFileResponseRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=0)]
        public string requestId;
        
        public getNistFileResponseRequest() {
        }
        
        public getNistFileResponseRequest(string requestId) {
            this.requestId = requestId;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getNistFileResponseResponse", WrapperNamespace="http://transaction.ws.adapter.edi.bodega.morphotrak.com/", IsWrapped=true)]
    public partial class getNistFileResponseResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=0)]
        public Mockservice.AfisDijin.NistResponse responseInfo;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=1)]
        [System.Xml.Serialization.XmlElementAttribute(DataType="hexBinary")]
        public byte[] nistFile;
        
        public getNistFileResponseResponse() {
        }
        
        public getNistFileResponseResponse(Mockservice.AfisDijin.NistResponse responseInfo, byte[] nistFile) {
            this.responseInfo = responseInfo;
            this.nistFile = nistFile;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface TransactionChannel : Mockservice.AfisDijin.Transaction, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class submitNistFileCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public submitNistFileCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public Mockservice.AfisDijin.submitNistFileResponse Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((Mockservice.AfisDijin.submitNistFileResponse)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class getNistFileResponseCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public getNistFileResponseCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public Mockservice.AfisDijin.getNistFileResponseResponse Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((Mockservice.AfisDijin.getNistFileResponseResponse)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class TransactionClient : System.ServiceModel.ClientBase<Mockservice.AfisDijin.Transaction>, Mockservice.AfisDijin.Transaction {
        
        private BeginOperationDelegate onBeginsubmitNistFileDelegate;
        
        private EndOperationDelegate onEndsubmitNistFileDelegate;
        
        private System.Threading.SendOrPostCallback onsubmitNistFileCompletedDelegate;
        
        private BeginOperationDelegate onBegingetNistFileResponseDelegate;
        
        private EndOperationDelegate onEndgetNistFileResponseDelegate;
        
        private System.Threading.SendOrPostCallback ongetNistFileResponseCompletedDelegate;
        
        public TransactionClient() {
        }
        
        public TransactionClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public TransactionClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public TransactionClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public TransactionClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public event System.EventHandler<submitNistFileCompletedEventArgs> submitNistFileCompleted;
        
        public event System.EventHandler<getNistFileResponseCompletedEventArgs> getNistFileResponseCompleted;
        
        public Mockservice.AfisDijin.submitNistFileResponse submitNistFile(Mockservice.AfisDijin.submitNistFileRequest request) {
            return base.Channel.submitNistFile(request);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public System.IAsyncResult BeginsubmitNistFile(Mockservice.AfisDijin.submitNistFileRequest request, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginsubmitNistFile(request, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public Mockservice.AfisDijin.submitNistFileResponse EndsubmitNistFile(System.IAsyncResult result) {
            return base.Channel.EndsubmitNistFile(result);
        }
        
        private System.IAsyncResult OnBeginsubmitNistFile(object[] inValues, System.AsyncCallback callback, object asyncState) {
            Mockservice.AfisDijin.submitNistFileRequest request = ((Mockservice.AfisDijin.submitNistFileRequest)(inValues[0]));
            return this.BeginsubmitNistFile(request, callback, asyncState);
        }
        
        private object[] OnEndsubmitNistFile(System.IAsyncResult result) {
            Mockservice.AfisDijin.submitNistFileResponse retVal = this.EndsubmitNistFile(result);
            return new object[] {
                    retVal};
        }
        
        private void OnsubmitNistFileCompleted(object state) {
            if ((this.submitNistFileCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.submitNistFileCompleted(this, new submitNistFileCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void submitNistFileAsync(Mockservice.AfisDijin.submitNistFileRequest request) {
            this.submitNistFileAsync(request, null);
        }
        
        public void submitNistFileAsync(Mockservice.AfisDijin.submitNistFileRequest request, object userState) {
            if ((this.onBeginsubmitNistFileDelegate == null)) {
                this.onBeginsubmitNistFileDelegate = new BeginOperationDelegate(this.OnBeginsubmitNistFile);
            }
            if ((this.onEndsubmitNistFileDelegate == null)) {
                this.onEndsubmitNistFileDelegate = new EndOperationDelegate(this.OnEndsubmitNistFile);
            }
            if ((this.onsubmitNistFileCompletedDelegate == null)) {
                this.onsubmitNistFileCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnsubmitNistFileCompleted);
            }
            base.InvokeAsync(this.onBeginsubmitNistFileDelegate, new object[] {
                        request}, this.onEndsubmitNistFileDelegate, this.onsubmitNistFileCompletedDelegate, userState);
        }
        
        public Mockservice.AfisDijin.getNistFileResponseResponse getNistFileResponse(Mockservice.AfisDijin.getNistFileResponseRequest request) {
            return base.Channel.getNistFileResponse(request);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public System.IAsyncResult BegingetNistFileResponse(Mockservice.AfisDijin.getNistFileResponseRequest request, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BegingetNistFileResponse(request, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public Mockservice.AfisDijin.getNistFileResponseResponse EndgetNistFileResponse(System.IAsyncResult result) {
            return base.Channel.EndgetNistFileResponse(result);
        }
        
        private System.IAsyncResult OnBegingetNistFileResponse(object[] inValues, System.AsyncCallback callback, object asyncState) {
            Mockservice.AfisDijin.getNistFileResponseRequest request = ((Mockservice.AfisDijin.getNistFileResponseRequest)(inValues[0]));
            return this.BegingetNistFileResponse(request, callback, asyncState);
        }
        
        private object[] OnEndgetNistFileResponse(System.IAsyncResult result) {
            Mockservice.AfisDijin.getNistFileResponseResponse retVal = this.EndgetNistFileResponse(result);
            return new object[] {
                    retVal};
        }
        
        private void OngetNistFileResponseCompleted(object state) {
            if ((this.getNistFileResponseCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.getNistFileResponseCompleted(this, new getNistFileResponseCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void getNistFileResponseAsync(Mockservice.AfisDijin.getNistFileResponseRequest request) {
            this.getNistFileResponseAsync(request, null);
        }
        
        public void getNistFileResponseAsync(Mockservice.AfisDijin.getNistFileResponseRequest request, object userState) {
            if ((this.onBegingetNistFileResponseDelegate == null)) {
                this.onBegingetNistFileResponseDelegate = new BeginOperationDelegate(this.OnBegingetNistFileResponse);
            }
            if ((this.onEndgetNistFileResponseDelegate == null)) {
                this.onEndgetNistFileResponseDelegate = new EndOperationDelegate(this.OnEndgetNistFileResponse);
            }
            if ((this.ongetNistFileResponseCompletedDelegate == null)) {
                this.ongetNistFileResponseCompletedDelegate = new System.Threading.SendOrPostCallback(this.OngetNistFileResponseCompleted);
            }
            base.InvokeAsync(this.onBegingetNistFileResponseDelegate, new object[] {
                        request}, this.onEndgetNistFileResponseDelegate, this.ongetNistFileResponseCompletedDelegate, userState);
        }
    }
}
