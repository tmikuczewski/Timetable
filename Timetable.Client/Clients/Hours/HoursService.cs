﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ten kod został wygenerowany przez narzędzie.
//     Wersja wykonawcza:4.0.30319.42000
//
//     Zmiany w tym pliku mogą spowodować nieprawidłowe zachowanie i zostaną utracone, jeśli
//     kod zostanie ponownie wygenerowany.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Timetable.Client.Clients.Hours
{
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
	[System.Runtime.Serialization.DataContractAttribute(Name="hours", Namespace="http://schemas.datacontract.org/2004/07/Timetable.DAL.Model")]
	public partial class hours : object, System.Runtime.Serialization.IExtensibleDataObject
	{
		
		private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
		
		private System.TimeSpan beginField;
		
		private System.TimeSpan endField;
		
		private int idField;
		
		private Timetable.DAL.Model.lessons_places[] lessons_placesField;
		
		private int numberField;
		
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData
		{
			get
			{
				return this.extensionDataField;
			}
			set
			{
				this.extensionDataField = value;
			}
		}
		
		[System.Runtime.Serialization.DataMemberAttribute()]
		public System.TimeSpan begin
		{
			get
			{
				return this.beginField;
			}
			set
			{
				this.beginField = value;
			}
		}
		
		[System.Runtime.Serialization.DataMemberAttribute()]
		public System.TimeSpan end
		{
			get
			{
				return this.endField;
			}
			set
			{
				this.endField = value;
			}
		}
		
		[System.Runtime.Serialization.DataMemberAttribute()]
		public int id
		{
			get
			{
				return this.idField;
			}
			set
			{
				this.idField = value;
			}
		}
		
		[System.Runtime.Serialization.DataMemberAttribute()]
		public Timetable.DAL.Model.lessons_places[] lessons_places
		{
			get
			{
				return this.lessons_placesField;
			}
			set
			{
				this.lessons_placesField = value;
			}
		}
		
		[System.Runtime.Serialization.DataMemberAttribute()]
		public int number
		{
			get
			{
				return this.numberField;
			}
			set
			{
				this.numberField = value;
			}
		}
	}
	
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
	[System.Runtime.Serialization.DataContractAttribute(Name="lessons_places", Namespace="http://schemas.datacontract.org/2004/07/Timetable.DAL.Model")]
	public partial class lessons_places : object, System.Runtime.Serialization.IExtensibleDataObject
	{
		
		private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
		
		private int classroomField;
		
		private Timetable.DAL.Model.classrooms classroomsField;
		
		private int dayField;
		
		private Timetable.DAL.Model.days daysField;
		
		private int hourField;
		
		private Timetable.DAL.Model.hours hoursField;
		
		private int lessonField;
		
		private Timetable.DAL.Model.lessons lessonsField;
		
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData
		{
			get
			{
				return this.extensionDataField;
			}
			set
			{
				this.extensionDataField = value;
			}
		}
		
		[System.Runtime.Serialization.DataMemberAttribute()]
		public int classroom
		{
			get
			{
				return this.classroomField;
			}
			set
			{
				this.classroomField = value;
			}
		}
		
		[System.Runtime.Serialization.DataMemberAttribute()]
		public Timetable.DAL.Model.classrooms classrooms
		{
			get
			{
				return this.classroomsField;
			}
			set
			{
				this.classroomsField = value;
			}
		}
		
		[System.Runtime.Serialization.DataMemberAttribute()]
		public int day
		{
			get
			{
				return this.dayField;
			}
			set
			{
				this.dayField = value;
			}
		}
		
		[System.Runtime.Serialization.DataMemberAttribute()]
		public Timetable.DAL.Model.days days
		{
			get
			{
				return this.daysField;
			}
			set
			{
				this.daysField = value;
			}
		}
		
		[System.Runtime.Serialization.DataMemberAttribute()]
		public int hour
		{
			get
			{
				return this.hourField;
			}
			set
			{
				this.hourField = value;
			}
		}
		
		[System.Runtime.Serialization.DataMemberAttribute()]
		public Timetable.DAL.Model.hours hours
		{
			get
			{
				return this.hoursField;
			}
			set
			{
				this.hoursField = value;
			}
		}
		
		[System.Runtime.Serialization.DataMemberAttribute()]
		public int lesson
		{
			get
			{
				return this.lessonField;
			}
			set
			{
				this.lessonField = value;
			}
		}
		
		[System.Runtime.Serialization.DataMemberAttribute()]
		public Timetable.DAL.Model.lessons lessons
		{
			get
			{
				return this.lessonsField;
			}
			set
			{
				this.lessonsField = value;
			}
		}
	}
	
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
	[System.Runtime.Serialization.DataContractAttribute(Name="classrooms", Namespace="http://schemas.datacontract.org/2004/07/Timetable.DAL.Model")]
	public partial class classrooms : object, System.Runtime.Serialization.IExtensibleDataObject
	{
		
		private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
		
		private string administratorField;
		
		private int idField;
		
		private Timetable.DAL.Model.lessons_places[] lessons_placesField;
		
		private string nameField;
		
		private Timetable.DAL.Model.teachers teachersField;
		
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData
		{
			get
			{
				return this.extensionDataField;
			}
			set
			{
				this.extensionDataField = value;
			}
		}
		
		[System.Runtime.Serialization.DataMemberAttribute()]
		public string administrator
		{
			get
			{
				return this.administratorField;
			}
			set
			{
				this.administratorField = value;
			}
		}
		
		[System.Runtime.Serialization.DataMemberAttribute()]
		public int id
		{
			get
			{
				return this.idField;
			}
			set
			{
				this.idField = value;
			}
		}
		
		[System.Runtime.Serialization.DataMemberAttribute()]
		public Timetable.DAL.Model.lessons_places[] lessons_places
		{
			get
			{
				return this.lessons_placesField;
			}
			set
			{
				this.lessons_placesField = value;
			}
		}
		
		[System.Runtime.Serialization.DataMemberAttribute()]
		public string name
		{
			get
			{
				return this.nameField;
			}
			set
			{
				this.nameField = value;
			}
		}
		
		[System.Runtime.Serialization.DataMemberAttribute()]
		public Timetable.DAL.Model.teachers teachers
		{
			get
			{
				return this.teachersField;
			}
			set
			{
				this.teachersField = value;
			}
		}
	}
	
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
	[System.Runtime.Serialization.DataContractAttribute(Name="days", Namespace="http://schemas.datacontract.org/2004/07/Timetable.DAL.Model")]
	public partial class days : object, System.Runtime.Serialization.IExtensibleDataObject
	{
		
		private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
		
		private int idField;
		
		private Timetable.DAL.Model.lessons_places[] lessons_placesField;
		
		private string nameField;
		
		private int numberField;
		
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData
		{
			get
			{
				return this.extensionDataField;
			}
			set
			{
				this.extensionDataField = value;
			}
		}
		
		[System.Runtime.Serialization.DataMemberAttribute()]
		public int id
		{
			get
			{
				return this.idField;
			}
			set
			{
				this.idField = value;
			}
		}
		
		[System.Runtime.Serialization.DataMemberAttribute()]
		public Timetable.DAL.Model.lessons_places[] lessons_places
		{
			get
			{
				return this.lessons_placesField;
			}
			set
			{
				this.lessons_placesField = value;
			}
		}
		
		[System.Runtime.Serialization.DataMemberAttribute()]
		public string name
		{
			get
			{
				return this.nameField;
			}
			set
			{
				this.nameField = value;
			}
		}
		
		[System.Runtime.Serialization.DataMemberAttribute()]
		public int number
		{
			get
			{
				return this.numberField;
			}
			set
			{
				this.numberField = value;
			}
		}
	}
	
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
	[System.Runtime.Serialization.DataContractAttribute(Name="lessons", Namespace="http://schemas.datacontract.org/2004/07/Timetable.DAL.Model")]
	public partial class lessons : object, System.Runtime.Serialization.IExtensibleDataObject
	{
		
		private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
		
		private int _classField;
		
		private Timetable.DAL.Model.classes classesField;
		
		private int idField;
		
		private Timetable.DAL.Model.lessons_places[] lessons_placesField;
		
		private int subjectField;
		
		private Timetable.DAL.Model.subjects subjectsField;
		
		private string teacherField;
		
		private Timetable.DAL.Model.teachers teachersField;
		
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData
		{
			get
			{
				return this.extensionDataField;
			}
			set
			{
				this.extensionDataField = value;
			}
		}
		
		[System.Runtime.Serialization.DataMemberAttribute()]
		public int _class
		{
			get
			{
				return this._classField;
			}
			set
			{
				this._classField = value;
			}
		}
		
		[System.Runtime.Serialization.DataMemberAttribute()]
		public Timetable.DAL.Model.classes classes
		{
			get
			{
				return this.classesField;
			}
			set
			{
				this.classesField = value;
			}
		}
		
		[System.Runtime.Serialization.DataMemberAttribute()]
		public int id
		{
			get
			{
				return this.idField;
			}
			set
			{
				this.idField = value;
			}
		}
		
		[System.Runtime.Serialization.DataMemberAttribute()]
		public Timetable.DAL.Model.lessons_places[] lessons_places
		{
			get
			{
				return this.lessons_placesField;
			}
			set
			{
				this.lessons_placesField = value;
			}
		}
		
		[System.Runtime.Serialization.DataMemberAttribute()]
		public int subject
		{
			get
			{
				return this.subjectField;
			}
			set
			{
				this.subjectField = value;
			}
		}
		
		[System.Runtime.Serialization.DataMemberAttribute()]
		public Timetable.DAL.Model.subjects subjects
		{
			get
			{
				return this.subjectsField;
			}
			set
			{
				this.subjectsField = value;
			}
		}
		
		[System.Runtime.Serialization.DataMemberAttribute()]
		public string teacher
		{
			get
			{
				return this.teacherField;
			}
			set
			{
				this.teacherField = value;
			}
		}
		
		[System.Runtime.Serialization.DataMemberAttribute()]
		public Timetable.DAL.Model.teachers teachers
		{
			get
			{
				return this.teachersField;
			}
			set
			{
				this.teachersField = value;
			}
		}
	}
	
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
	[System.Runtime.Serialization.DataContractAttribute(Name="teachers", Namespace="http://schemas.datacontract.org/2004/07/Timetable.DAL.Model")]
	public partial class teachers : object, System.Runtime.Serialization.IExtensibleDataObject
	{
		
		private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
		
		private Timetable.DAL.Model.classes[] classesField;
		
		private Timetable.DAL.Model.classrooms[] classroomsField;
		
		private string first_nameField;
		
		private string last_nameField;
		
		private Timetable.DAL.Model.lessons[] lessonsField;
		
		private string peselField;
		
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData
		{
			get
			{
				return this.extensionDataField;
			}
			set
			{
				this.extensionDataField = value;
			}
		}
		
		[System.Runtime.Serialization.DataMemberAttribute()]
		public Timetable.DAL.Model.classes[] classes
		{
			get
			{
				return this.classesField;
			}
			set
			{
				this.classesField = value;
			}
		}
		
		[System.Runtime.Serialization.DataMemberAttribute()]
		public Timetable.DAL.Model.classrooms[] classrooms
		{
			get
			{
				return this.classroomsField;
			}
			set
			{
				this.classroomsField = value;
			}
		}
		
		[System.Runtime.Serialization.DataMemberAttribute()]
		public string first_name
		{
			get
			{
				return this.first_nameField;
			}
			set
			{
				this.first_nameField = value;
			}
		}
		
		[System.Runtime.Serialization.DataMemberAttribute()]
		public string last_name
		{
			get
			{
				return this.last_nameField;
			}
			set
			{
				this.last_nameField = value;
			}
		}
		
		[System.Runtime.Serialization.DataMemberAttribute()]
		public Timetable.DAL.Model.lessons[] lessons
		{
			get
			{
				return this.lessonsField;
			}
			set
			{
				this.lessonsField = value;
			}
		}
		
		[System.Runtime.Serialization.DataMemberAttribute()]
		public string pesel
		{
			get
			{
				return this.peselField;
			}
			set
			{
				this.peselField = value;
			}
		}
	}
	
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
	[System.Runtime.Serialization.DataContractAttribute(Name="classes", Namespace="http://schemas.datacontract.org/2004/07/Timetable.DAL.Model")]
	public partial class classes : object, System.Runtime.Serialization.IExtensibleDataObject
	{
		
		private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
		
		private string code_nameField;
		
		private int idField;
		
		private Timetable.DAL.Model.lessons[] lessonsField;
		
		private Timetable.DAL.Model.students[] studentsField;
		
		private Timetable.DAL.Model.teachers teachersField;
		
		private string tutorField;
		
		private int yearField;
		
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData
		{
			get
			{
				return this.extensionDataField;
			}
			set
			{
				this.extensionDataField = value;
			}
		}
		
		[System.Runtime.Serialization.DataMemberAttribute()]
		public string code_name
		{
			get
			{
				return this.code_nameField;
			}
			set
			{
				this.code_nameField = value;
			}
		}
		
		[System.Runtime.Serialization.DataMemberAttribute()]
		public int id
		{
			get
			{
				return this.idField;
			}
			set
			{
				this.idField = value;
			}
		}
		
		[System.Runtime.Serialization.DataMemberAttribute()]
		public Timetable.DAL.Model.lessons[] lessons
		{
			get
			{
				return this.lessonsField;
			}
			set
			{
				this.lessonsField = value;
			}
		}
		
		[System.Runtime.Serialization.DataMemberAttribute()]
		public Timetable.DAL.Model.students[] students
		{
			get
			{
				return this.studentsField;
			}
			set
			{
				this.studentsField = value;
			}
		}
		
		[System.Runtime.Serialization.DataMemberAttribute()]
		public Timetable.DAL.Model.teachers teachers
		{
			get
			{
				return this.teachersField;
			}
			set
			{
				this.teachersField = value;
			}
		}
		
		[System.Runtime.Serialization.DataMemberAttribute()]
		public string tutor
		{
			get
			{
				return this.tutorField;
			}
			set
			{
				this.tutorField = value;
			}
		}
		
		[System.Runtime.Serialization.DataMemberAttribute()]
		public int year
		{
			get
			{
				return this.yearField;
			}
			set
			{
				this.yearField = value;
			}
		}
	}
	
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
	[System.Runtime.Serialization.DataContractAttribute(Name="students", Namespace="http://schemas.datacontract.org/2004/07/Timetable.DAL.Model")]
	public partial class students : object, System.Runtime.Serialization.IExtensibleDataObject
	{
		
		private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
		
		private System.Nullable<int> _classField;
		
		private Timetable.DAL.Model.classes classesField;
		
		private string first_nameField;
		
		private string last_nameField;
		
		private string peselField;
		
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData
		{
			get
			{
				return this.extensionDataField;
			}
			set
			{
				this.extensionDataField = value;
			}
		}
		
		[System.Runtime.Serialization.DataMemberAttribute()]
		public System.Nullable<int> _class
		{
			get
			{
				return this._classField;
			}
			set
			{
				this._classField = value;
			}
		}
		
		[System.Runtime.Serialization.DataMemberAttribute()]
		public Timetable.DAL.Model.classes classes
		{
			get
			{
				return this.classesField;
			}
			set
			{
				this.classesField = value;
			}
		}
		
		[System.Runtime.Serialization.DataMemberAttribute()]
		public string first_name
		{
			get
			{
				return this.first_nameField;
			}
			set
			{
				this.first_nameField = value;
			}
		}
		
		[System.Runtime.Serialization.DataMemberAttribute()]
		public string last_name
		{
			get
			{
				return this.last_nameField;
			}
			set
			{
				this.last_nameField = value;
			}
		}
		
		[System.Runtime.Serialization.DataMemberAttribute()]
		public string pesel
		{
			get
			{
				return this.peselField;
			}
			set
			{
				this.peselField = value;
			}
		}
	}
	
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
	[System.Runtime.Serialization.DataContractAttribute(Name="subjects", Namespace="http://schemas.datacontract.org/2004/07/Timetable.DAL.Model")]
	public partial class subjects : object, System.Runtime.Serialization.IExtensibleDataObject
	{
		
		private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
		
		private int idField;
		
		private Timetable.DAL.Model.lessons[] lessonsField;
		
		private string nameField;
		
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData
		{
			get
			{
				return this.extensionDataField;
			}
			set
			{
				this.extensionDataField = value;
			}
		}
		
		[System.Runtime.Serialization.DataMemberAttribute()]
		public int id
		{
			get
			{
				return this.idField;
			}
			set
			{
				this.idField = value;
			}
		}
		
		[System.Runtime.Serialization.DataMemberAttribute()]
		public Timetable.DAL.Model.lessons[] lessons
		{
			get
			{
				return this.lessonsField;
			}
			set
			{
				this.lessonsField = value;
			}
		}
		
		[System.Runtime.Serialization.DataMemberAttribute()]
		public string name
		{
			get
			{
				return this.nameField;
			}
			set
			{
				this.nameField = value;
			}
		}
	}


	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
	[System.ServiceModel.ServiceContractAttribute(ConfigurationName="IHourService")]
	public interface IHourService
	{
	
		[System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IHourService/GetAllHours", ReplyAction="http://tempuri.org/IHourService/GetAllHoursResponse")]
		Timetable.DAL.Model.hours[] GetAllHours();
	
		[System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IHourService/GetAllHours", ReplyAction="http://tempuri.org/IHourService/GetAllHoursResponse")]
		System.Threading.Tasks.Task<Timetable.DAL.Model.hours[]> GetAllHoursAsync();
	}

	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
	public interface IHourServiceChannel : IHourService, System.ServiceModel.IClientChannel
	{
	}

	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
	public partial class HourServiceClient : System.ServiceModel.ClientBase<IHourService>, IHourService
	{
	
		public HourServiceClient()
		{
		}
	
		public HourServiceClient(string endpointConfigurationName) : 
			base(endpointConfigurationName)
		{
		}
	
		public HourServiceClient(string endpointConfigurationName, string remoteAddress) : 
			base(endpointConfigurationName, remoteAddress)
		{
		}
	
		public HourServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
			base(endpointConfigurationName, remoteAddress)
		{
		}
	
		public HourServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
			base(binding, remoteAddress)
		{
		}
	
		public Timetable.DAL.Model.hours[] GetAllHours()
		{
			return base.Channel.GetAllHours();
		}
	
		public System.Threading.Tasks.Task<Timetable.DAL.Model.hours[]> GetAllHoursAsync()
		{
			return base.Channel.GetAllHoursAsync();
		}
	}
}