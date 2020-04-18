/*
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using UnityEditor;

namespace Sisus
{
	[InitializeOnLoad]
	public static class MemberVisibility
	{
		private static Dictionary<FieldInfo, FieldVisibilityInfo> fieldVisibility;
		private static Dictionary<PropertyInfo, FieldVisibilityInfo> propertyVisibility;
		private static Dictionary<MethodInfo, FieldVisibilityInfo> methodVisibility;
		private static Dictionary<Type, List<MemberInfo>> MembersByType;

		private static bool setupDone;

		static MemberVisibility()
		{
			ThreadPool.QueueUserWorkItem(SetupThreaded);
		}

		private static void SetupThreaded(object threadTaskId)
		{
			fieldVisibility = new Dictionary<FieldInfo, FieldVisibilityInfo>(8192);
			propertyVisibility = new Dictionary<PropertyInfo, FieldVisibilityInfo>(8192);
			methodVisibility = new Dictionary<MethodInfo, FieldVisibilityInfo>(4096);
			MembersByType = new Dictionary<Type, List<MemberInfo>>(4096);

			var generatedFields = new HashSet<string>();

			foreach(var classType in TypeExtensions.GetAllTypesThreadSafe(true, true, true))
			{
				var type = classType;

				if(!TypeExtensions.TypeIsSafe(classType))
				{
					continue;
				}

				var members = new List<MemberInfo>(32);

				do
				{
					var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly);
					for(int n = 0, count = fields.Length; n < count; n++)
					{
						var field = fields[n];

						//don't show obsolete fields, even if hidden are included
						//since accessing them can cause errors to be thrown etc.
						if(AttributeUtility.HasAttributes<ObsoleteAttribute>(field))
						{
							fieldVisibility.Add(field, FieldVisibilityInfo.NeverShown);
							continue;
						}

						//don't show cyclical fields to avoid infinite loop
						if(field.FieldType == field.DeclaringType && !Types.UnityObject.IsAssignableFrom(field.FieldType))
						{
							fieldVisibility.Add(field, FieldVisibilityInfo.NeverShown);
							continue;
						}

						// Property backing fields should never be shown. It makes more sense to show the properties themselves instead.
						if(field.IsPropertyBackingField())
						{
							fieldVisibility.Add(field, FieldVisibilityInfo.NeverShown);
							continue;
						}

						bool attributeSetVisibility;
						if(MemberInfoExtensions.TryGetAttributeDeterminedInspectorVisibility(field, out attributeSetVisibility, true))
						{
							if(attributeSetVisibility)
							{
								fieldVisibility.Add(field, FieldVisibilityInfo.AlwaysShown);
								continue;
							}

							// Static fields are only shown if attributes are used to expose them.
							if(field.IsStatic)
							{
								fieldVisibility.Add(field, FieldVisibilityInfo.Static);
								continue;
							}

							// Only has the NonSerialized attribute.
							if(!MemberInfoExtensions.TryGetAttributeDeterminedInspectorVisibility(field, out attributeSetVisibility, false))
							{
								if(field.IsPublic)
								{
									fieldVisibility.Add(field, FieldVisibilityInfo.PublicNonSerialized);
									continue;
								}
								fieldVisibility.Add(field, FieldVisibilityInfo.NonPublicNonSerialized);
								continue;
							}

							fieldVisibility.Add(field, FieldVisibilityInfo.Hidden);
							continue;
						}

						// Static fields are only shown if attributes are used to expose them.
						if(field.IsStatic)
						{
							fieldVisibility.Add(field, FieldVisibilityInfo.Static);
							continue;
						}

						if(field.IsPublic)
						{
							// public fields are shown by default only if the field type class has the Serializable attribute
							var fieldType = field.FieldType;
							if(fieldType.IsSerializable)
							{
								if(fieldType.IsGenericType)
								{
									var genericType = fieldType.GetGenericTypeDefinition();

									//Unity can't handle serialization of generic types, with the exception of List,
									//so don't display them by default, even if they're public, to avoid confusion
									//they can still be forced visible using the SerializeField tag
									if(genericType == Types.List)
									{
										var listType = fieldType.GetGenericArguments()[0];
										//TO DO: Should check the generic type recursively
										//return listType.IsInspectorViewable();
										return !listType.IsGenericType && (listType.IsSerializable || listType.Assembly == Types.UnityAssembly);
									}

									return false;
								}

								if(type == Types.DateTime || type == Types.TimeSpan)
								{
									return false;
								}
					
								return true;
							}
				
							//for some reason these are not marked as serializable
							//but as a special case Unity can still serialize them
							//(and displays them in the inspector)
							//if(type == typeof(AnimationCurve)
							//|| type == typeof(Color)
							//|| type == typeof(Color32)
							//|| type == typeof(AnimationClip))
							if(type.Assembly == Types.UnityAssembly)
							{
								return true;
							}
							var baseType = type.BaseType;
							while(baseType != null)
							{
								if(baseType.Assembly == Types.UnityAssembly)
								{
									return true;
								}
								baseType = baseType.BaseType;
							}

							return false;
						}

						return showNonSerialized == FieldVisibility.AllExceptHidden;





						if(field.IsInspectorViewable(true, FieldVisibility.AllPublic))
						{
							members.Add(field);

							// Don't display hidden fields twice except in debug mode
							if(!generatedFields.Add(field.Name) || !field.IsInspectorViewable(false, FieldVisibility.AllPublic))
							{
								fieldVisibility.Add(field, FieldVisibilityInfo.Hidden);
								continue;
							}

							if(!field.IsInspectorViewable(false, FieldVisibility.))
							{
								fieldVisibility.Add(field, FieldVisibilityInfo.Hidden);
							}
							else
								!field.IsInspectorViewable(false, FieldVisibility.AllPublic))
						}
						else
						{
							fieldVisibility.Add(field, FieldVisibilityInfo.NeverShown);
						}
					}
					type = type.BaseType;
				}
				// We stop once we hit specific base types (even in debug mode this is desired, to avoid the number of results getting out of hand)
				while(type != Types.MonoBehaviour && type != Types.Component && type != Types.ScriptableObject && type != null && type != Types.UnityObject && type != Types.SystemObject);

				generatedFields.Clear();
			}

			setupDone = true;
		}

		//[Flags]
		//public enum FieldVisibilityInfo : byte
		//{
		//	None = 0,
		//	IsPublic = (1 << 1),
		//	IsSerialized = (1 << 2),
		//	IsHidden = (1 << 3)
		//}

		public enum FieldVisibilityInfo : byte
		{
			NeverShown = 0,

			Hidden = 1,
			NonPublicNonSerialized = 2,
			PublicNonSerialized = 3,
			AlwaysShown = 4,

			Static = 5
		}

		//Serialized and not hidden = show always (most)
		//Public non-serialized, non-hidden = show in debug mode and with FieldVisbility.AllPublic
		//Private non-serialized = show in debug mode and with FieldVisbility.AllPublic
		//Hidden = Show in debug mode only (second least)
		//Obsolete / backlisted = Show never (least)

		public static bool ShowInInspector(FieldInfo field, bool includeHidden, FieldVisibility fieldVisibility)
		{
			if(!setupDone)
			{
				return field.IsInspectorViewable(includeHidden, fieldVisibility);
			}

			MemberVisibility.TryGetValue

			return IsVisibleInInspector()
		}

		private static bool IsVisibleInInspector(FieldVisibilityInfo info, bool includeHidden, FieldVisibility fieldVisibility)
		{
			switch(info)
			{
				case FieldVisibilityInfo.NeverShown:
					return false;
				case FieldVisibilityInfo.Hidden:
					return includeHidden;
				case FieldVisibilityInfo.NonPublicNonSerialized:
					return fieldVisibility == MemberVisibility.fieldVisibility.AllPublic || fieldVisibility == MemberVisibility.fieldVisibility.AllExceptHidden || includeHidden;
				case FieldVisibilityInfo.PublicNonSerialized:
					return fieldVisibility == MemberVisibility.fieldVisibility.AllPublic || includeHidden;
				default: //FieldVisibilityInfo.Serialized:
					return true;
			}
		}
	}
}
*/