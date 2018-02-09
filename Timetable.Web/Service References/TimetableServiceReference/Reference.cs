﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Timetable.Web.TimetableServiceReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="TimetableServiceReference.ITimetableService")]
    public interface ITimetableService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITimetableService/GetTimetableEntities", ReplyAction="http://tempuri.org/ITimetableService/GetTimetableEntitiesResponse")]
        Timetable.DAL.ViewModels.TimetableViewModel GetTimetableEntities();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITimetableService/GetTimetableEntities", ReplyAction="http://tempuri.org/ITimetableService/GetTimetableEntitiesResponse")]
        System.Threading.Tasks.Task<Timetable.DAL.ViewModels.TimetableViewModel> GetTimetableEntitiesAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITimetableService/GetTimetableForClass", ReplyAction="http://tempuri.org/ITimetableService/GetTimetableForClassResponse")]
        Timetable.DAL.ViewModels.TimetableViewModel GetTimetableForClass(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITimetableService/GetTimetableForClass", ReplyAction="http://tempuri.org/ITimetableService/GetTimetableForClassResponse")]
        System.Threading.Tasks.Task<Timetable.DAL.ViewModels.TimetableViewModel> GetTimetableForClassAsync(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITimetableService/GetTimetableForTeacher", ReplyAction="http://tempuri.org/ITimetableService/GetTimetableForTeacherResponse")]
        Timetable.DAL.ViewModels.TimetableViewModel GetTimetableForTeacher(string pesel);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITimetableService/GetTimetableForTeacher", ReplyAction="http://tempuri.org/ITimetableService/GetTimetableForTeacherResponse")]
        System.Threading.Tasks.Task<Timetable.DAL.ViewModels.TimetableViewModel> GetTimetableForTeacherAsync(string pesel);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITimetableService/GetTimetableForClassroom", ReplyAction="http://tempuri.org/ITimetableService/GetTimetableForClassroomResponse")]
        Timetable.DAL.ViewModels.TimetableViewModel GetTimetableForClassroom(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITimetableService/GetTimetableForClassroom", ReplyAction="http://tempuri.org/ITimetableService/GetTimetableForClassroomResponse")]
        System.Threading.Tasks.Task<Timetable.DAL.ViewModels.TimetableViewModel> GetTimetableForClassroomAsync(int id);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ITimetableServiceChannel : Timetable.Web.TimetableServiceReference.ITimetableService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class TimetableServiceClient : System.ServiceModel.ClientBase<Timetable.Web.TimetableServiceReference.ITimetableService>, Timetable.Web.TimetableServiceReference.ITimetableService {
        
        public TimetableServiceClient() {
        }
        
        public TimetableServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public TimetableServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public TimetableServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public TimetableServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public Timetable.DAL.ViewModels.TimetableViewModel GetTimetableEntities() {
            return base.Channel.GetTimetableEntities();
        }
        
        public System.Threading.Tasks.Task<Timetable.DAL.ViewModels.TimetableViewModel> GetTimetableEntitiesAsync() {
            return base.Channel.GetTimetableEntitiesAsync();
        }
        
        public Timetable.DAL.ViewModels.TimetableViewModel GetTimetableForClass(int id) {
            return base.Channel.GetTimetableForClass(id);
        }
        
        public System.Threading.Tasks.Task<Timetable.DAL.ViewModels.TimetableViewModel> GetTimetableForClassAsync(int id) {
            return base.Channel.GetTimetableForClassAsync(id);
        }
        
        public Timetable.DAL.ViewModels.TimetableViewModel GetTimetableForTeacher(string pesel) {
            return base.Channel.GetTimetableForTeacher(pesel);
        }
        
        public System.Threading.Tasks.Task<Timetable.DAL.ViewModels.TimetableViewModel> GetTimetableForTeacherAsync(string pesel) {
            return base.Channel.GetTimetableForTeacherAsync(pesel);
        }
        
        public Timetable.DAL.ViewModels.TimetableViewModel GetTimetableForClassroom(int id) {
            return base.Channel.GetTimetableForClassroom(id);
        }
        
        public System.Threading.Tasks.Task<Timetable.DAL.ViewModels.TimetableViewModel> GetTimetableForClassroomAsync(int id) {
            return base.Channel.GetTimetableForClassroomAsync(id);
        }
    }
}
