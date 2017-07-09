object("SkillBar")
{
	recttransform(0.000, 0.000, 0.000, 0.000, 0.000)
	{
		anchor(0.000, 0.000, 0.000, 0.000);
		pivot(0.000, 0.000);
		rotation(0.000, 0.000, 0.000);
		scale(0.000, 0.000, 0.000);
		offset(0.000, 0.000, 0.000, 0.000);
	};
	component("GraphicRaycaster", "UnityEngine.UI.GraphicRaycaster, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
	component("CanvasScaler", "UnityEngine.UI.CanvasScaler, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
	object("Panel")
	{
		recttransform(0.000, -244.000, 0.000, 0.000, -488.000)
		{
			anchor(0.000, 0.000, 1.000, 1.000);
			pivot(0.500, 0.500);
			rotation(0.000, 0.000, 0.000);
			scale(1.000, 1.000, 1.000);
			offset(0.000, 0.000, 0.000, -488.000);
		};
		component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
		{
			sprite("Assets/Resources/UITexture/ActorBigIcon.png");
			color(1.000, 1.000, 1.000, 0.392);
		};
		object("ImageBkg")
		{
			recttransform(0.000, 0.000, 0.000, 1149.000, 116.000)
			{
				anchor(0.500, 0.500, 0.500, 0.500);
				pivot(0.500, 0.500);
				rotation(0.000, 0.000, 0.000);
				scale(1.000, 1.000, 1.000);
				offset(-574.500, -58.000, 574.500, 58.000);
			};
			component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
			{
				sprite("Assets/Resources/UITexture/ActorIcons.png");
				color(1.000, 1.000, 1.000, 1.000);
			};
		};
		object("PanelSkills")
		{
			recttransform(0.000, 0.000, 0.000, 1149.000, 116.000)
			{
				anchor(0.500, 0.500, 0.500, 0.500);
				pivot(0.500, 0.500);
				rotation(0.000, 0.000, 0.000);
				scale(1.000, 1.000, 1.000);
				offset(-574.500, -58.000, 574.500, 58.000);
			};
			component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
			{
				sprite("Resources/unity_builtin_extra");
				color(1.000, 1.000, 1.000, 0.392);
			};
		};
	};
};
