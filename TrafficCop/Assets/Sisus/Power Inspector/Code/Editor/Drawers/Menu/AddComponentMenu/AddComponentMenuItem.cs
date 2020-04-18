using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Sisus
{
	public class AddComponentMenuItem
	{
		public string label;
		public Type type;
		[NonSerialized]
		public AddComponentMenuItem parent;
		public AddComponentMenuItem[] children;
		private Texture preview;
		private bool previewFetched;

		public bool IsGroup
		{
			get
			{
				return type == null;
			}
		}

		public Texture Preview
		{
			get
			{
				if(!previewFetched)
				{
					UpdatePreview();
				}

				return preview;
			}
		}

		[CanBeNull]
		public string Namespace
		{
			get
			{
				if(type != null)
				{
					return type.Namespace;
				}
				
				for(int n = children.Length - 1; n >= 0; n--)
				{
					var childNamespace = children[n].Namespace;
					if(!string.IsNullOrEmpty(childNamespace))
					{
						return childNamespace;
					}
				}

				return null;
			}
		}

		private void UpdatePreview()
		{
			previewFetched = true;

			#if UNITY_EDITOR
			if(IsGroup)
			{
				var child = GetFirstNonGroupChild();
				if(child != null)
				{
					var source = child.Preview as Texture2D;
					preview = TextureUtility.Resize(source, 15, 15);
				}
				return;
			}

			UnityEditor.EditorGUIUtility.SetIconSize(Vector2.zero);
			preview = UnityEditor.AssetPreview.GetMiniTypeThumbnail(type);
			
			if(preview == null && Types.MonoBehaviour.IsAssignableFrom(type))
			{
				preview = InspectorUtility.Preferences.graphics.CSharpScriptIcon;
			}

			if(preview != null)
			{
				preview.filterMode = FilterMode.Point;
			}
			#endif
		}

		private AddComponentMenuItem(){}

		public static AddComponentMenuItem Group(string groupLabel, AddComponentMenuItem setParent, Texture setPreview = null)
		{
			#if DEV_MODE && PI_ASSERTATIONS
			if(string.Equals(groupLabel, "Unity Engine"))
			{
				Debug.LogError("AddComponentMenu Group \""+ groupLabel + "\" being created.");
			}
			#endif

			var group = new AddComponentMenuItem();
			group.label = groupLabel;
			group.parent = setParent;
			group.children = new AddComponentMenuItem[0];
			group.preview = setPreview;
			group.previewFetched = setPreview != null;
			return group;
		}
		
		public static AddComponentMenuItem Item([NotNull]Type setType, string classLabel, [CanBeNull]AddComponentMenuItem setParent)
		{
			#if DEV_MODE && PI_ASSERTATIONS
			if(setParent != null && string.Equals(setParent.label, "Unity Engine"))
			{
				Debug.LogError("AddComponentMenu Item \"" + classLabel + "\" being created under \"Unity Engine\" group. Type should be added under a more descriptive Group in InspectorPreferences.addComponentMenuConfig.");
			}
			Debug.Assert(setType != null);
			#endif

			var item = new AddComponentMenuItem();
			item.parent = setParent;
			item.label = classLabel;
			item.type = setType;
			return item;
		}

		public bool Contains(Type targetType)
		{
			if(children != null)
			{
				for(int n = children.Length - 1; n >= 0; n--)
				{
					if(children[n].Contains(targetType))
					{
						return true;
					}
				}
				return false;
			}
			return type == targetType;
		}

		public void AddChild(string classLabel, Type setType)
		{
			#if DEV_MODE && PI_ASSERTATIONS
			if(string.Equals(label, "Unity Engine"))
			{
				Debug.LogError("AddComponentMenu Item \"" + classLabel + "\" being created under \"Unity Engine\" group. Type should be added under a more descriptive Group in InspectorPreferences.addComponentMenuConfig.");
			}
			if(setType.IsBaseComponentType())
			{
				Debug.LogError("AddComponentMenu Item \"" + classLabel + "\" type "+setType+" was a base component type and should not be shown in the add component menu.");
			}
			#endif

			#if DEV_MODE && PI_ASSERTATIONS
			int countBeforeAdd = children.Length;
			#endif

			var addItem = Item(setType, classLabel, this);
			ArrayExtensions.Add(ref children, addItem);

			#if DEV_MODE && PI_ASSERTATIONS
			Debug.Assert(children.Length == countBeforeAdd + 1);
			#endif
		}
		
		public string FullLabel()
		{
			if(parent != null)
			{
				return string.Concat(parent.FullLabel(), "/", label);
			}
			return label;
		}

		public void GetClassLabelsFlattened(ref List<string> labels, ref Dictionary<string, AddComponentMenuItem> items)
		{
			if(children == null)
			{
				string fullLabel = FullLabel();
				labels.Add(fullLabel);
				items[fullLabel] = this;
				return;
			}

			for(int n = children.Length - 1; n >= 0; n--)
			{
				children[n].GetClassLabelsFlattened(ref labels, ref items);
			}
		}

		[CanBeNull]
		public Type TypeOrAnyChildType()
		{
			if(type != null)
			{
				return type;
			}

			for(int n = children.Length - 1; n >= 0; n--)
			{
				var childType = children[n].type;
				if(childType != null)
				{
					return childType;
				}
			}

			return null;
		}

		[CanBeNull]
		public AddComponentMenuItem GetFirstNonGroupChild()
		{
			int count = children.Length;

			for(int n = 0; n < count; n++)
			{
				var child = children[n];
				if(!child.IsGroup)
				{
					return child;
				}
			}

			for(int n = 0; n < count; n++)
			{
				var child = children[n].GetFirstNonGroupChild();
				if(child != null)
				{
					return child;
				}
			}

			return null;
		}

		public override string ToString()
		{
			return label;
		}
	}
}