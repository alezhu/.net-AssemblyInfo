using System;
using System.Reflection;
using System.IO;
namespace azCS {
	class AssemblyInfo
	{
		private static string OutputDir = null;
		private static string AssemblyName = null;
		private static string RootPath = null;

		private static string GetAssemblyName() {
			Console.Write("\nEnter assembly name [or empty string for exit]:");
			return Console.ReadLine();
		}

		private static void ProcessProperties(Type ty, StreamWriter stream) {
			PropertyInfo[] properties = ty.GetProperties();
			if (properties.Length>0)
			{
				stream.WriteLine("<properties>");
				foreach(PropertyInfo property in properties) {
					ProcessProperty(property,stream);
				}
				stream.WriteLine("</properties>");
			}
		}

		private static void ProcessProperty(PropertyInfo property, StreamWriter stream){
			stream.WriteLine(String.Format("<item name=\"{0}\">",property.Name));
			stream.WriteLine(String.Format("<definition>{0}</definition>",property.ToString()));
			stream.WriteLine(String.Format("<attributes>{0}</attributes>",property.Attributes.ToString()));
			ParameterInfo[] indexParameters = property.GetIndexParameters();
			if (indexParameters.Length>0)
			{
				stream.WriteLine("<indexes>");
				foreach(ParameterInfo index in indexParameters) 
				{
					stream.WriteLine(String.Format("<index>{0} {1}</index>",index.ParameterType.ToString(), index.Name));
				}
				stream.WriteLine("</indexes>");
			}
			ProcessCustomAttributes(property, stream);
			stream.WriteLine(String.Format("<canread>{0}</canread>",(property.CanRead)?1:0));
			stream.WriteLine(String.Format("<canwrite>{0}</canwrite>",(property.CanWrite)?1:0));
			stream.WriteLine(String.Format("<isspecialname>{0}</isspecialname>",(property.IsSpecialName)?1:0));
			stream.WriteLine(String.Format("<membertype>{0}</membertype>",property.MemberType.ToString()));
			stream.WriteLine(String.Format("<declaringtype href=\"{1}{2}\">{0}</declaringtype>",property.DeclaringType.FullName,RootPath,GetPathFromType(property.DeclaringType)));
			stream.WriteLine(String.Format("<propertytype href=\"{1}{2}\">{0}</propertytype>",property.PropertyType.FullName,RootPath,GetPathFromType(property.PropertyType)));
			stream.WriteLine(String.Format("<reflectedtype href=\"{1}{2}\">{0}</reflectedtype>",property.ReflectedType.FullName,RootPath,GetPathFromType(property.ReflectedType)));
			MethodInfo me = property.GetGetMethod();
			stream.WriteLine(String.Format("<getmethod>{0}</getmethod>",(me != null)?me.Name:""));
			me = property.GetSetMethod();
			stream.WriteLine(String.Format("<setmethod>{0}</setmethod>",(me != null)?me.Name:""));
			stream.WriteLine("</item>");
		}

		private static void ProcessFields(Type ty, StreamWriter stream) {
			FieldInfo[] fields = ty.GetFields();
			if (fields.Length>0)
			{
				stream.WriteLine("<fields>");
				foreach(FieldInfo field in fields) {
					ProcessField(field,stream);
				}
				stream.WriteLine("</fields>");
			}
		}

		private static void ProcessField(FieldInfo field, StreamWriter stream){
			stream.WriteLine(String.Format("<item name=\"{0}\">",field.Name));
			stream.WriteLine(String.Format("<definition>{0}</definition>",field.ToString()));
			stream.WriteLine(String.Format("<attributes>{0}</attributes>",field.Attributes.ToString()));
			stream.WriteLine(String.Format("<declaringtype href=\"{1}{2}\">{0}</declaringtype>",field.DeclaringType.FullName,RootPath,GetPathFromType(field.DeclaringType)));
			stream.WriteLine(String.Format("<fieldtype href=\"{1}{2}\">{0}</fieldtype>",field.FieldType .FullName,RootPath,GetPathFromType(field.FieldType)));
			ProcessCustomAttributes(field, stream);
			stream.WriteLine(String.Format("<isassembly>{0}</isassembly>",(field.IsAssembly)?1:0));
			stream.WriteLine(String.Format("<isfamily>{0}</isfamily>",(field.IsFamily)?1:0));
			stream.WriteLine(String.Format("<isfamilyandassembly>{0}</isfamilyandassembly>",(field.IsFamilyAndAssembly)?1:0));
			stream.WriteLine(String.Format("<isfamilyorassembly>{0}</isfamilyorassembly>",(field.IsFamilyOrAssembly)?1:0));
			stream.WriteLine(String.Format("<isinitonly>{0}</isinitonly>",(field.IsInitOnly)?1:0));
			stream.WriteLine(String.Format("<isliteral>{0}</isliteral>",(field.IsLiteral)?1:0));
			stream.WriteLine(String.Format("<isnotserialized>{0}</isnotserialized>",(field.IsNotSerialized)?1:0));
			stream.WriteLine(String.Format("<ispinvokeimpl>{0}</ispinvokeimpl>",(field.IsPinvokeImpl)?1:0));
			stream.WriteLine(String.Format("<isprivate>{0}</isprivate>",(field.IsPrivate)?1:0));
			stream.WriteLine(String.Format("<ispublic>{0}</ispublic>",(field.IsPublic)?1:0));
			stream.WriteLine(String.Format("<isspecialname>{0}</isspecialname>",(field.IsSpecialName)?1:0));
			stream.WriteLine(String.Format("<isstatic>{0}</isstatic>",(field.IsStatic)?1:0));
			stream.WriteLine(String.Format("<reflectedtype href=\"{1}{2}\">{0}</reflectedtype>",field.ReflectedType.FullName,RootPath,GetPathFromType(field.ReflectedType)));
			stream.WriteLine("</item>");
		}

		private static void ProcessMethods(Type ty, StreamWriter stream) {
			MethodInfo[] Methods = ty.GetMethods();
			if (Methods.Length>0)
			{
				stream.WriteLine("<methods>");
				foreach(MethodInfo Method in Methods) {
					ProcessMethod(Method,stream);
				}
				stream.WriteLine("</methods>");
			}
		}

		private static void ProcessMethod(MethodInfo Method, StreamWriter stream){
			if (Method == null){
				return;
			}
			stream.WriteLine(String.Format("<item name=\"{0}\">",Method.Name));
			stream.WriteLine(String.Format("<definition>{0}</definition>",Method.ToString()));
			stream.WriteLine(String.Format("<attributes>{0}</attributes>",Method.Attributes.ToString()));
			stream.WriteLine(String.Format("<declaringtype href=\"{1}{2}\">{0}</declaringtype>",Method.DeclaringType.FullName,RootPath,GetPathFromType(Method.DeclaringType)));
			ProcessCustomAttributes(Method, stream);
			stream.WriteLine(String.Format("<isabstract>{0}</isabstract>",(Method.IsAbstract)?1:0));
			stream.WriteLine(String.Format("<isassembly>{0}</isassembly>",(Method.IsAssembly)?1:0));
			stream.WriteLine(String.Format("<isconstructor>{0}</isconstructor>",(Method.IsConstructor)?1:0));
			stream.WriteLine(String.Format("<isfamily>{0}</isfamily>",(Method.IsFamily)?1:0));
			stream.WriteLine(String.Format("<isfamilyandassembly>{0}</isfamilyandassembly>",(Method.IsFamilyAndAssembly)?1:0));
			stream.WriteLine(String.Format("<isfamilyorassembly>{0}</isfamilyorassembly>",(Method.IsFamilyOrAssembly)?1:0));
			stream.WriteLine(String.Format("<isfinal>{0}</isfinal>",(Method.IsFinal)?1:0));
			stream.WriteLine(String.Format("<ishidebysig>{0}</ishidebysig>",(Method.IsHideBySig)?1:0));
			stream.WriteLine(String.Format("<isprivate>{0}</isprivate>",(Method.IsPrivate)?1:0));
			stream.WriteLine(String.Format("<ispublic>{0}</ispublic>",(Method.IsPublic)?1:0));
			stream.WriteLine(String.Format("<isspecialname>{0}</isspecialname>",(Method.IsSpecialName)?1:0));
			stream.WriteLine(String.Format("<isstatic>{0}</isstatic>",(Method.IsStatic)?1:0));
			stream.WriteLine(String.Format("<isvirtual>{0}</isvirtual>",(Method.IsVirtual)?1:0));
			stream.WriteLine(String.Format("<reflectedtype href=\"{1}{2}\">{0}</reflectedtype>",Method.ReflectedType.FullName,RootPath,GetPathFromType(Method.ReflectedType)));
			stream.WriteLine(String.Format("<returntype href=\"{1}{2}\">{0}</returntype>",Method.ReturnType.FullName,RootPath,GetPathFromType(Method.ReturnType)));
			stream.WriteLine("<returntypeattributes>");
			ProcessCustomAttributes(Method.ReturnTypeCustomAttributes, stream);
			stream.WriteLine("</returntypeattributes>");
			stream.WriteLine(String.Format("<methodimplattributes>{0}</methodimplattributes>",Method.GetMethodImplementationFlags().ToString()));
			ProcessParameters(Method,stream);
			stream.WriteLine("</item>");
		}


		private static void ProcessParameters(MethodBase member,StreamWriter stream) {
			ParameterInfo[] Parameters = member.GetParameters();
			if (Parameters.Length > 0)
			{
				stream.WriteLine("<parameters>");
				foreach(ParameterInfo Parameter in Parameters) {
					ProcessParameter(Parameter, stream);
				}
				stream.WriteLine("</parameters>");
			}
		}

		private static void ProcessParameter(ParameterInfo parameter,StreamWriter stream) {
			stream.WriteLine(String.Format("<item name=\"{0}\">",parameter.Name));
			stream.WriteLine(String.Format("<definition>{0}</definition>",parameter.ToString()));
			stream.WriteLine(String.Format("<attributes>{0}</attributes>",parameter.Attributes.ToString()));
			ProcessCustomAttributes(parameter, stream);
			stream.WriteLine(String.Format("<defaultvalue>{0}</defaultvalue>",parameter.DefaultValue.ToString()));
			stream.WriteLine(String.Format("<isin>{0}</isin>",(parameter.IsIn)?1:0));
			stream.WriteLine(String.Format("<islcid>{0}</islcid>",(parameter.IsLcid)?1:0));
			stream.WriteLine(String.Format("<isoptional>{0}</isoptional>",(parameter.IsOptional)?1:0));
			stream.WriteLine(String.Format("<isout>{0}</isout>",(parameter.IsOut)?1:0));
			stream.WriteLine(String.Format("<isretval>{0}</isretval>",(parameter.IsRetval)?1:0));
			Type ty = parameter.ParameterType;
			stream.WriteLine(String.Format("<isbyref>{0}</isbyref>",(ty.IsByRef)?1:0));
			stream.WriteLine(String.Format("<isarray>{0}</isarray>",(ty.IsArray)?1:0));
			stream.WriteLine(String.Format("<parametertype href=\"{1}{2}\">{0}</parametertype>",ty.FullName.Replace("&","&amp;"),RootPath,GetPathFromType(ty)));
			stream.WriteLine(String.Format("<position>{0}</position>",parameter.Position.ToString()));
			stream.WriteLine("</item>");
		}



		private static void ProcessEvents(Type ty, StreamWriter stream) {
			EventInfo[] Events = ty.GetEvents();
			if (Events.Length>0)
			{
				stream.WriteLine("<events>");
				foreach(EventInfo Event in Events) {
					ProcessEvent(Event,stream);
				}
				stream.WriteLine("</events>");
			}
		}

		private static void ProcessEvent(EventInfo Event, StreamWriter stream){
			stream.WriteLine(String.Format("<item name=\"{0}\">",Event.Name));
			stream.WriteLine(String.Format("<definition>{0}</definition>",Event.ToString()));
			stream.WriteLine(String.Format("<attributes>{0}</attributes>",Event.Attributes.ToString()));
			stream.WriteLine(String.Format("<declaringtype href=\"{1}{2}\">{0}</declaringtype>",Event.DeclaringType.FullName,RootPath,GetPathFromType(Event.DeclaringType)));
			ProcessCustomAttributes(Event, stream);
			stream.WriteLine("<eventhandlertype href=\"{1}{2}\">{0}</eventhandlertype >",Event.EventHandlerType.FullName,RootPath,GetPathFromType(Event.EventHandlerType));
			stream.WriteLine(String.Format("<ismulticast>{0}</ismulticast>",(Event.IsMulticast)?1:0));
			stream.WriteLine(String.Format("<isspecialname>{0}</isspecialname>",(Event.IsSpecialName)?1:0));
			stream.WriteLine("<membertypes>{0}</membertypes>",Event.MemberType.ToString());
			stream.WriteLine(String.Format("<reflectedtype href=\"{1}{2}\">{0}</reflectedtype>",Event.ReflectedType.FullName,RootPath,GetPathFromType(Event.ReflectedType)));
			stream.WriteLine("<addmethod>");
				ProcessMethod(Event.GetAddMethod(),stream);
			stream.WriteLine("</addmethod>");
			stream.WriteLine("<raisemethod>");
				ProcessMethod(Event.GetRaiseMethod(),stream);
			stream.WriteLine("</raisemethod>");
			stream.WriteLine("<removemethod>");
				ProcessMethod(Event.GetRemoveMethod(),stream);
			stream.WriteLine("</removemethod>");
			stream.WriteLine("</item>");
		}

		private static void ProcessEvent2(EventInfo Event, StreamWriter stream){
			stream.WriteLine(String.Format("<item name=\"{0}\">",Event.Name));
			stream.WriteLine(String.Format("<definition>{0}</definition>",Event.ToString()));
			stream.WriteLine(String.Format("<attributes>{0}</attributes>",Event.Attributes.ToString()));
			ProcessCustomAttributes(Event, stream);
			stream.WriteLine("</item>");
		}

		private static void ProcessInterfaces(Type ty, StreamWriter stream) {
			Type[] Interfaces = ty.GetInterfaces();
			if (Interfaces.Length>0)
			{
				stream.WriteLine("<interfaces>");
				foreach(Type Interface in Interfaces) {
					ProcessInterface(Interface,stream);
				}
				stream.WriteLine("</interfaces>");
			}
		}

		private static void ProcessInterface(Type Interface, StreamWriter stream){
			stream.WriteLine(String.Format("<item name=\"{0}\">",Interface.Name));
			stream.WriteLine(String.Format("<definition>{0}</definition>",Interface.ToString()));
			stream.WriteLine(String.Format("<attributes>{0}</attributes>",Interface.Attributes.ToString()));
			stream.WriteLine("<interfacetype href=\"{1}{2}\">{0}</interfacetype>",Interface.FullName,RootPath,GetPathFromType(Interface));
			ProcessCustomAttributes(Interface, stream);
			stream.WriteLine("</item>");
		}

		private static void ProcessInterface2(Type Interface, StreamWriter stream){
			stream.WriteLine(String.Format("<item name=\"{0}\">",Interface.Name));
			stream.WriteLine(String.Format("<definition>{0}</definition>",Interface.ToString()));
			stream.WriteLine(String.Format("<attributes>{0}</attributes>",Interface.Attributes.ToString()));
			ProcessCustomAttributes(Interface, stream);
			stream.WriteLine("</item>");
		}
		private static void ProcessConstructors(Type ty, StreamWriter stream) {
			ConstructorInfo[] Constructors = ty.GetConstructors();
			if (Constructors.Length>0)
			{
				stream.WriteLine("<constructors>");
				foreach(ConstructorInfo Constructor in Constructors) {
					ProcessConstructor(Constructor,stream);
				}
				stream.WriteLine("</constructors>");
			}
		}

		private static void ProcessConstructor(ConstructorInfo Constructor, StreamWriter stream){
			stream.WriteLine(String.Format("<item name=\"{0}\">",Constructor.Name));
			stream.WriteLine(String.Format("<definition>{0}</definition>",Constructor.ToString()));
			stream.WriteLine(String.Format("<attributes>{0}</attributes>",Constructor.Attributes.ToString()));
			stream.WriteLine(String.Format("<declaringtype href=\"{1}{2}\">{0}</declaringtype>",Constructor.DeclaringType.Name,RootPath,GetPathFromType(Constructor.DeclaringType)));
			ProcessCustomAttributes(Constructor, stream);
			stream.WriteLine(String.Format("<isabstract>{0}</isabstract>",(Constructor.IsAbstract)?1:0));
			stream.WriteLine(String.Format("<isassembly>{0}</isassembly>",(Constructor.IsAssembly)?1:0));
			stream.WriteLine(String.Format("<isconstructor>{0}</isconstructor>",(Constructor.IsConstructor)?1:0));
			stream.WriteLine(String.Format("<isfamily>{0}</isfamily>",(Constructor.IsFamily)?1:0));
			stream.WriteLine(String.Format("<isfamilyandassembly>{0}</isfamilyandassembly>",(Constructor.IsFamilyAndAssembly)?1:0));
			stream.WriteLine(String.Format("<isfamilyorassembly>{0}</isfamilyorassembly>",(Constructor.IsFamilyOrAssembly)?1:0));
			stream.WriteLine(String.Format("<isfinal>{0}</isfinal>",(Constructor.IsFinal)?1:0));
			stream.WriteLine(String.Format("<ishidebysig>{0}</ishidebysig>",(Constructor.IsHideBySig)?1:0));
			stream.WriteLine(String.Format("<isprivate>{0}</isprivate>",(Constructor.IsPrivate)?1:0));
			stream.WriteLine(String.Format("<ispublic>{0}</ispublic>",(Constructor.IsPublic)?1:0));
			stream.WriteLine(String.Format("<isspecialname>{0}</isspecialname>",(Constructor.IsSpecialName)?1:0));
			stream.WriteLine(String.Format("<isstatic>{0}</isstatic>",(Constructor.IsStatic)?1:0));
			stream.WriteLine(String.Format("<isvirtual>{0}</isvirtual>",(Constructor.IsVirtual)?1:0));
			stream.WriteLine(String.Format("<reflectedtype href=\"{1}{2}\">{0}</reflectedtype>",Constructor.ReflectedType.FullName,RootPath,GetPathFromType(Constructor.ReflectedType)));
			stream.WriteLine(String.Format("<methodimplattributes>{0}</methodimplattributes>",Constructor.GetMethodImplementationFlags().ToString()));
			ProcessParameters(Constructor,stream);
			stream.WriteLine("</item>");
		}

		private static void ProcessCustomAttributes(Object obj, StreamWriter stream) {
			MemberInfo mi = (MemberInfo)obj;
			Object[] CustomAttributes = mi.GetCustomAttributes(true);
			if (CustomAttributes.Length>0)
			{
				stream.WriteLine("<customattributes>");
				foreach(Object CustomAttribute in CustomAttributes) {
					string fn  = CustomAttribute.ToString();
					stream.WriteLine(String.Format("<item href=\"{1}{2}\">{0}</item>",fn,RootPath,GetPathFromTypeStr(fn)));
				}
				stream.WriteLine("</customattributes>");
			}
		}

		private static void ProcessCustomAttributes(ICustomAttributeProvider provider, StreamWriter stream) {
			ICustomAttributeProvider mi = provider;
			Object[] CustomAttributes = mi.GetCustomAttributes(true);
			if (CustomAttributes.Length>0)
			{
				stream.WriteLine("<customattributes>");
				foreach(Object CustomAttribute in CustomAttributes) {
					string fn  = CustomAttribute.ToString();
					stream.WriteLine(String.Format("<item href=\"{1}{2}\">{0}</item>",fn,RootPath,GetPathFromTypeStr(fn)));
				}
				stream.WriteLine("</customattributes>");
			}
		}


		private static string GetPathToRoot(Type AType) {
			return GetPathToRoot(AType.FullName);
		}

		private static string GetPathToRoot(string FullName) {
			string Path = "./";
			int p = 0;
			while ((p = FullName.IndexOf('.',p)) > 0)
			{
				p++;
				Path += "../";
			}
			return Path;
			
		}

		private static string GetPathFromType(Type AType) {
			return GetPathFromTypeStr(AType.FullName);
		}

		private static string GetPathFromTypeStr(string FullName) {
			String Path = null;
			if (FullName.IndexOf(".") > 0) {
				Path = FullName.Replace(".","/");
			} else if (FullName != "Nothing") {
				Path = "System/" + FullName;
			} else {
				Path = FullName;
			}
			return Path.Replace("&","").Replace("[]","");
		}

		private static void ProcessType(Type ty) {
			string FullName = ty.FullName;
			RootPath = GetPathToRoot(ty);
			Console.WriteLine(FullName);
			//strings[] nameParts = ty.FullName.Split('.');
			string FilePath = OutputDir + FullName.Replace('.','\\');
			if (!Directory.Exists(Path.GetDirectoryName(FilePath)))
			{
				Directory.CreateDirectory(Path.GetDirectoryName(FilePath));
			}
			using (System.IO.StreamWriter stream = System.IO.File.CreateText(FilePath+".xml")) {
				stream.WriteLine("<?xml version=\"1.0\" encoding=\"Windows-1251\" standalone=\"yes\" ?>");
				stream.WriteLine(String.Format("<?xml-stylesheet type=\"text/xsl\" href=\"{0}ai.xsl\"?>",RootPath));
				stream.WriteLine(String.Format("<type>{3}<fullname>{0}</fullname>{3}<assembly>{1}</assembly>{3}<namespace>{2}</namespace>",FullName,ty.Assembly,ty.Namespace,
					stream.NewLine));
				Type baseType = ty.BaseType;
				if (baseType == null)
				{
					stream.WriteLine("<base>Nothing</base>");
				} else {
					stream.WriteLine(String.Format("<base href=\"{1}{2}\">{0}</base>",baseType.FullName,RootPath,GetPathFromType(baseType)));
				}

				ProcessConstructors(ty,stream);
				ProcessProperties(ty,stream);
				ProcessFields(ty,stream);
				ProcessMethods(ty,stream);
				ProcessEvents(ty,stream);
				ProcessInterfaces(ty,stream);

				stream.WriteLine("</type>");
				stream.Close();
			}
		}

		private  static void ProcessAssembly(string AssemblyName) {
			try
			{
				//Assembly assembly = AppDomain.CurrentDomain.Load(AssemblyName);
				Assembly assembly = Assembly.LoadWithPartialName(AssemblyName);
				if (assembly != null)
				{
					RootPath = GetPathToRoot(AssemblyName)+@"../";
					Console.WriteLine("Ass Root: {0}",RootPath);
					string FilePath = Path.Combine(OutputDir , AssemblyName.Replace('.',Path.DirectorySeparatorChar));
					Console.WriteLine("OutputDir: {0}",OutputDir );
					Console.WriteLine("FilePath: {0}",FilePath);
					if (!Directory.Exists(FilePath))
					{
						Directory.CreateDirectory(FilePath);
					}
					FilePath = Path.Combine(FilePath,String.Format("_assembly {0}.xml",AssemblyName));
					using (System.IO.StreamWriter stream = System.IO.File.CreateText(FilePath)) {
						stream.WriteLine("<?xml version=\"1.0\" encoding=\"Windows-1251\" standalone=\"yes\" ?>");
						stream.WriteLine("<?xml-stylesheet type=\"text/xsl\" href=\"{0}ai.xsl\"?>",RootPath);
						stream.WriteLine("<assembly>{2}<fullname>{0}</fullname>{2}<assembly>{1}</assembly>{2}",
							AssemblyName,assembly.ToString(),stream.NewLine);

						stream.WriteLine("<codebase>{0}</codebase>",assembly.CodeBase);
						stream.WriteLine("<escapedcodebase>{0}</escapedcodebase>",assembly.EscapedCodeBase);
						stream.WriteLine("<entrypoint>");
						ProcessMethod(assembly.EntryPoint,stream);
						stream.WriteLine("</entrypoint>");
						stream.WriteLine("<gac>{0}</gac>",(assembly.GlobalAssemblyCache)?1:0);
						stream.WriteLine("<imageruntimeversion>{0}</imageruntimeversion>",assembly.ImageRuntimeVersion);
						stream.WriteLine("<location>{0}</location>",assembly.Location);
						ProcessCustomAttributes(assembly as ICustomAttributeProvider, stream);
						stream.WriteLine("<references>");
						AssemblyName[] asms = assembly.GetReferencedAssemblies();
						foreach (AssemblyName aname in asms){
							stream.WriteLine("<reference href=\"{1}{2}\">{0}</reference>",aname.FullName,RootPath,GetPathFromTypeStr(aname.Name));
						}
						stream.WriteLine("</references>");
						stream.WriteLine("</assembly>");
						stream.Close();
					}

					Type[] types = assembly.GetExportedTypes();
					//Type[] types = assembly.GetTypes();
					foreach (Type ty in types) {
						ProcessType(ty);
					}
				}else {
					Console.WriteLine(String.Format("Assembly {0} Not Found.",AssemblyName));
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}
		}

		private static void ProcessArgs() {
			//OutputDir = @"d:\user_WS\AZ\temp\CS\AI\";
			OutputDir = Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
			if (OutputDir == String.Empty){
				OutputDir = Environment.CurrentDirectory;
			}
			OutputDir += Path.DirectorySeparatorChar;
			Console.WriteLine("OutputDir: {0}",OutputDir );
			AssemblyName = null;
		}

		public static void Main() {
			Console.WriteLine("AssemblyInfo v1.0\nCopyright © 2007 by AZ");
			ProcessArgs();
			if (AssemblyName != null)
			{
				ProcessAssembly(AssemblyName);
			} else {
				while ( "" != (AssemblyName = GetAssemblyName()) )
				{
					ProcessAssembly(AssemblyName);
				}
			}
		}
	};
}