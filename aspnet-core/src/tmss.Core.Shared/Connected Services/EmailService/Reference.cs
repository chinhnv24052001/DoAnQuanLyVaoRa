﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EmailService
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://toyotavn.com.vn/", ConfigurationName="EmailService.EmailServiceSoap")]
    public interface EmailServiceSoap
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://toyotavn.com.vn/SendEmail", ReplyAction="*")]
        System.Threading.Tasks.Task<EmailService.SendEmailResponse> SendEmailAsync(EmailService.SendEmailRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://toyotavn.com.vn/DeleteFileServer", ReplyAction="*")]
        System.Threading.Tasks.Task<EmailService.DeleteFileServerResponse> DeleteFileServerAsync(EmailService.DeleteFileServerRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class SendEmailRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="SendEmail", Namespace="http://toyotavn.com.vn/", Order=0)]
        public EmailService.SendEmailRequestBody Body;
        
        public SendEmailRequest()
        {
        }
        
        public SendEmailRequest(EmailService.SendEmailRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://toyotavn.com.vn/")]
    public partial class SendEmailRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string from;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string to;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string cc;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string bcc;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=4)]
        public string subject;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=5)]
        public string body;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=6)]
        public string attachments;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=7)]
        public string connectionString;
        
        public SendEmailRequestBody()
        {
        }
        
        public SendEmailRequestBody(string from, string to, string cc, string bcc, string subject, string body, string attachments, string connectionString)
        {
            this.from = from;
            this.to = to;
            this.cc = cc;
            this.bcc = bcc;
            this.subject = subject;
            this.body = body;
            this.attachments = attachments;
            this.connectionString = connectionString;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class SendEmailResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="SendEmailResponse", Namespace="http://toyotavn.com.vn/", Order=0)]
        public EmailService.SendEmailResponseBody Body;
        
        public SendEmailResponse()
        {
        }
        
        public SendEmailResponse(EmailService.SendEmailResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://toyotavn.com.vn/")]
    public partial class SendEmailResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public bool SendEmailResult;
        
        public SendEmailResponseBody()
        {
        }
        
        public SendEmailResponseBody(bool SendEmailResult)
        {
            this.SendEmailResult = SendEmailResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class DeleteFileServerRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="DeleteFileServer", Namespace="http://toyotavn.com.vn/", Order=0)]
        public EmailService.DeleteFileServerRequestBody Body;
        
        public DeleteFileServerRequest()
        {
        }
        
        public DeleteFileServerRequest(EmailService.DeleteFileServerRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://toyotavn.com.vn/")]
    public partial class DeleteFileServerRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string fileName;
        
        public DeleteFileServerRequestBody()
        {
        }
        
        public DeleteFileServerRequestBody(string fileName)
        {
            this.fileName = fileName;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class DeleteFileServerResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="DeleteFileServerResponse", Namespace="http://toyotavn.com.vn/", Order=0)]
        public EmailService.DeleteFileServerResponseBody Body;
        
        public DeleteFileServerResponse()
        {
        }
        
        public DeleteFileServerResponse(EmailService.DeleteFileServerResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://toyotavn.com.vn/")]
    public partial class DeleteFileServerResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public bool DeleteFileServerResult;
        
        public DeleteFileServerResponseBody()
        {
        }
        
        public DeleteFileServerResponseBody(bool DeleteFileServerResult)
        {
            this.DeleteFileServerResult = DeleteFileServerResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    public interface EmailServiceSoapChannel : EmailService.EmailServiceSoap, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    public partial class EmailServiceSoapClient : System.ServiceModel.ClientBase<EmailService.EmailServiceSoap>, EmailService.EmailServiceSoap
    {
        
        /// <summary>
        /// Implement this partial method to configure the service endpoint.
        /// </summary>
        /// <param name="serviceEndpoint">The endpoint to configure</param>
        /// <param name="clientCredentials">The client credentials</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public EmailServiceSoapClient(EndpointConfiguration endpointConfiguration) : 
                base(EmailServiceSoapClient.GetBindingForEndpoint(endpointConfiguration), EmailServiceSoapClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public EmailServiceSoapClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(EmailServiceSoapClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public EmailServiceSoapClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(EmailServiceSoapClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public EmailServiceSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        public System.Threading.Tasks.Task<EmailService.SendEmailResponse> SendEmailAsync(EmailService.SendEmailRequest request)
        {
            return base.Channel.SendEmailAsync(request);
        }
        
        public System.Threading.Tasks.Task<EmailService.DeleteFileServerResponse> DeleteFileServerAsync(EmailService.DeleteFileServerRequest request)
        {
            return base.Channel.DeleteFileServerAsync(request);
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        public virtual System.Threading.Tasks.Task CloseAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginClose(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndClose));
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.EmailServiceSoap))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            if ((endpointConfiguration == EndpointConfiguration.EmailServiceSoap12))
            {
                System.ServiceModel.Channels.CustomBinding result = new System.ServiceModel.Channels.CustomBinding();
                System.ServiceModel.Channels.TextMessageEncodingBindingElement textBindingElement = new System.ServiceModel.Channels.TextMessageEncodingBindingElement();
                textBindingElement.MessageVersion = System.ServiceModel.Channels.MessageVersion.CreateVersion(System.ServiceModel.EnvelopeVersion.Soap12, System.ServiceModel.Channels.AddressingVersion.None);
                result.Elements.Add(textBindingElement);
                System.ServiceModel.Channels.HttpTransportBindingElement httpBindingElement = new System.ServiceModel.Channels.HttpTransportBindingElement();
                httpBindingElement.AllowCookies = true;
                httpBindingElement.MaxBufferSize = int.MaxValue;
                httpBindingElement.MaxReceivedMessageSize = int.MaxValue;
                result.Elements.Add(httpBindingElement);
                return result;
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.EmailServiceSoap))
            {
                return new System.ServiceModel.EndpointAddress("http://192.168.2.103:8888/SendEmail/EmailService.asmx");
            }
            if ((endpointConfiguration == EndpointConfiguration.EmailServiceSoap12))
            {
                return new System.ServiceModel.EndpointAddress("http://192.168.2.103:8888/SendEmail/EmailService.asmx");
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        public enum EndpointConfiguration
        {
            
            EmailServiceSoap,
            
            EmailServiceSoap12,
        }
    }
}
