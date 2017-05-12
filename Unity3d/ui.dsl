object("FlyBloodNum")
{
	recttransform(0.000, -48.400, 0.000, 150.000, 28.000)
	{
		anchor(0.500, 0.500, 0.500, 0.500);
		pivot(0.500, 0.500);
		rotation(0.000, 0.000, 0.000);
		scale(1.000, 1.000, 1.000);
		offset(-75.000, -62.400, 75.000, -34.400);
	};
	object("Panel")
	{
		recttransform(0.000, 0.000, 0.000, 0.000, 0.000)
		{
			anchor(0.000, 0.000, 1.000, 1.000);
			pivot(0.500, 0.500);
			rotation(0.000, 0.000, 0.000);
			scale(1.000, 1.000, 1.000);
			offset(0.000, 0.000, 0.000, 0.000);
		};
		component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
		{
			sprite("Resources/unity_builtin_extra");
			color(1.000, 1.000, 1.000, 0.392);
		};
		object("TextRed")
		{
			recttransform(0.000, 0.000, 0.000, 160.000, 30.000)
			{
				anchor(0.500, 0.500, 0.500, 0.500);
				pivot(0.500, 0.500);
				rotation(0.000, 0.000, 0.000);
				scale(0.700, 0.700, 1.000);
				offset(-80.000, -15.000, 80.000, 15.000);
			};
			component("Text", "UnityEngine.UI.Text, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
			{
				text("-39");
				color(1.000, 1.000, 1.000, 1.000);
				font("zhandou01", "Assets/UI/UITexture/zhandou01.fontsettings", 0, 1, "Normal");
				align("UpperCenter", False);
			};
		};
		object("TextGreen")
		{
			recttransform(0.000, 0.000, 0.000, 160.000, 30.000)
			{
				anchor(0.500, 0.500, 0.500, 0.500);
				pivot(0.500, 0.500);
				rotation(0.000, 0.000, 0.000);
				scale(0.700, 0.700, 1.000);
				offset(-80.000, -15.000, 80.000, 15.000);
			};
			component("Text", "UnityEngine.UI.Text, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
			{
				text("-39");
				color(1.000, 1.000, 1.000, 1.000);
				font("zhandou02", "Assets/UI/UITexture/zhandou02.fontsettings", 0, 1, "Normal");
				align("UpperCenter", False);
			};
		};
		object("TextWhite")
		{
			recttransform(0.000, 0.000, 0.000, 160.000, 30.000)
			{
				anchor(0.500, 0.500, 0.500, 0.500);
				pivot(0.500, 0.500);
				rotation(0.000, 0.000, 0.000);
				scale(0.700, 0.700, 1.000);
				offset(-80.000, -15.000, 80.000, 15.000);
			};
			component("Text", "UnityEngine.UI.Text, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
			{
				text("-39");
				color(1.000, 1.000, 1.000, 1.000);
				font("zhandou03", "Assets/UI/UITexture/zhandou03.fontsettings", 0, 1, "Normal");
				align("UpperCenter", False);
			};
		};
	};
};
object("HighlightPrompt")
{
	recttransform(0.000, 0.000, 0.000, 0.000, 0.000)
	{
		anchor(0.000, 0.000, 0.000, 0.000);
		pivot(0.000, 0.000);
		rotation(0.000, 0.000, 0.000);
		scale(0.000, 0.000, 0.000);
		offset(0.000, 0.000, 0.000, 0.000);
	};
	component("CanvasScaler", "UnityEngine.UI.CanvasScaler, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
	component("GraphicRaycaster", "UnityEngine.UI.GraphicRaycaster, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
};
object("HighlightText")
{
	recttransform(0.000, 0.000, 0.000, 160.000, 39.000)
	{
		anchor(0.500, 0.500, 0.500, 0.500);
		pivot(0.500, 0.500);
		rotation(0.000, 0.000, 0.000);
		scale(1.000, 1.000, 1.000);
		offset(-80.000, -19.500, 80.000, 19.500);
	};
	component("Text", "UnityEngine.UI.Text, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
	{
		text("中文试试");
		color(1.000, 1.000, 1.000, 1.000);
		font("Arial", "Library/unity default resources", 24, 1, "Bold");
		align("MiddleCenter", False);
	};
};
object("Image")
{
	recttransform(0.000, 0.000, 0.000, 0.000, 80.000)
	{
		anchor(0.000, 1.000, 0.000, 1.000);
		pivot(0.000, 1.000);
		rotation(0.000, 0.000, 0.000);
		scale(1.000, 1.000, 1.000);
		offset(0.000, -80.000, 0.000, 0.000);
	};
	component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
	{
		sprite("");
		color(0.129, 0.129, 0.129, 0.000);
	};
	object("Text")
	{
		recttransform(0.000, 0.000, 0.000, 0.000, 0.000)
		{
			anchor(0.000, 0.000, 1.000, 1.000);
			pivot(0.500, 0.500);
			rotation(0.000, 0.000, 0.000);
			scale(1.000, 1.000, 1.000);
			offset(0.000, 0.000, 0.000, 0.000);
		};
		component("Text", "UnityEngine.UI.Text, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
		{
			text("<color="#115500">fdsafdasfasd</color>fdsafdsafdsafdas<b>fdasfdasfds</b><i>fdasfdasfdsa</i>");
			color(0.986, 1.000, 0.000, 1.000);
			font("Arial", "Library/unity default resources", 18, 1, "Normal");
			align("UpperLeft", False);
		};
	};
};
object("Loading")
{
	recttransform(0.000, 0.000, 0.000, 0.000, 0.000)
	{
		anchor(0.000, 0.000, 0.000, 0.000);
		pivot(0.000, 0.000);
		rotation(0.000, 0.000, 0.000);
		scale(0.000, 0.000, 0.000);
		offset(0.000, 0.000, 0.000, 0.000);
	};
	component("CanvasScaler", "UnityEngine.UI.CanvasScaler, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
	component("GraphicRaycaster", "UnityEngine.UI.GraphicRaycaster, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
	object("ImgBackground")
	{
		recttransform(0.000, 0.000, 0.000, 800.000, 658.000)
		{
			anchor(0.500, 0.500, 0.500, 0.500);
			pivot(0.500, 0.500);
			rotation(0.000, 0.000, 0.000);
			scale(1.000, 1.000, 1.000);
			offset(-400.000, -329.000, 400.000, 329.000);
		};
		component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
		{
			sprite("Assets/UI/UITexture/tip_bg_s.png");
			color(0.000, 0.000, 0.000, 1.000);
		};
	};
	object("SldProgress")
	{
		recttransform(0.000, 102.000, 0.000, 720.000, 29.000)
		{
			anchor(0.500, 0.000, 0.500, 0.000);
			pivot(0.500, 0.500);
			rotation(0.000, 0.000, 0.000);
			scale(1.000, 1.000, 1.000);
			offset(-360.000, 87.500, 360.000, 116.500);
		};
		component("Slider", "UnityEngine.UI.Slider, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
		object("Background")
		{
			recttransform(0.000, 5.500, 0.000, 720.000, 40.000)
			{
				anchor(0.500, 0.500, 0.500, 0.500);
				pivot(0.500, 0.500);
				rotation(0.000, 0.000, 0.000);
				scale(1.000, 1.000, 1.000);
				offset(-360.000, -14.500, 360.000, 25.500);
			};
			component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
			{
				sprite("Assets/UI/UITexture/loading01.png");
				color(1.000, 1.000, 1.000, 1.000);
			};
		};
		object("Fill Area")
		{
			recttransform(360.000, 16.500, 0.000, 720.000, 25.000)
			{
				anchor(0.000, 0.000, 0.000, 0.000);
				pivot(0.500, 0.500);
				rotation(0.000, 0.000, 0.000);
				scale(1.000, 1.000, 1.000);
				offset(0.000, 4.000, 720.000, 29.000);
			};
			object("Fill")
			{
				recttransform(0.000, 0.000, 0.000, 0.000, 0.000)
				{
					anchor(0.000, 0.000, 0.000, 0.000);
					pivot(0.500, 0.500);
					rotation(0.000, 0.000, 0.000);
					scale(1.000, 1.000, 1.000);
					offset(0.000, 0.000, 0.000, 0.000);
				};
				component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
				{
					sprite("Assets/UI/UITexture/loading02.png");
					color(1.000, 1.000, 1.000, 1.000);
				};
			};
		};
	};
};
object("Login")
{
	recttransform(0.000, 0.000, 0.000, 0.000, 0.000)
	{
		anchor(0.000, 0.000, 0.000, 0.000);
		pivot(0.000, 0.000);
		rotation(0.000, 0.000, 0.000);
		scale(0.000, 0.000, 0.000);
		offset(0.000, 0.000, 0.000, 0.000);
	};
	component("CanvasScaler", "UnityEngine.UI.CanvasScaler, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
	component("GraphicRaycaster", "UnityEngine.UI.GraphicRaycaster, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
	object("Panel")
	{
		recttransform(-14.000, 25.000, 0.000, 362.000, 210.000)
		{
			anchor(0.500, 0.500, 0.500, 0.500);
			pivot(0.500, 0.500);
			rotation(0.000, 0.000, 0.000);
			scale(1.000, 1.000, 1.000);
			offset(-195.000, -80.000, 167.000, 130.000);
		};
		component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
		{
			sprite("Assets/UI/UITexture/Frame.png");
			color(1.000, 1.000, 1.000, 0.392);
		};
		object("AccountLabel")
		{
			recttransform(-79.000, 26.000, 0.000, 50.000, 24.000)
			{
				anchor(0.500, 0.500, 0.500, 0.500);
				pivot(0.500, 0.500);
				rotation(0.000, 0.000, 0.000);
				scale(1.000, 1.000, 1.000);
				offset(-104.000, 14.000, -54.000, 38.000);
			};
			component("Text", "UnityEngine.UI.Text, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
			{
				text("帐号：");
				color(0.196, 0.196, 0.196, 1.000);
				font("Arial", "Library/unity default resources", 14, 1, "Normal");
				align("UpperLeft", False);
			};
		};
		object("Account")
		{
			recttransform(26.000, 26.000, 0.000, 160.000, 30.000)
			{
				anchor(0.500, 0.500, 0.500, 0.500);
				pivot(0.500, 0.500);
				rotation(0.000, 0.000, 0.000);
				scale(1.000, 1.000, 1.000);
				offset(-54.000, 11.000, 106.000, 41.000);
			};
			component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
			{
				sprite("Resources/unity_builtin_extra");
				color(1.000, 1.000, 1.000, 1.000);
			};
			component("InputField", "UnityEngine.UI.InputField, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
			object("Placeholder")
			{
				recttransform(0.000, -0.500, 0.000, -20.000, -13.000)
				{
					anchor(0.000, 0.000, 1.000, 1.000);
					pivot(0.500, 0.500);
					rotation(0.000, 0.000, 0.000);
					scale(1.000, 1.000, 1.000);
					offset(10.000, 6.000, -10.000, -7.000);
				};
				component("Text", "UnityEngine.UI.Text, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
				{
					text("Enter text...");
					color(0.196, 0.196, 0.196, 0.500);
					font("Arial", "Library/unity default resources", 14, 1, "Italic");
					align("UpperLeft", False);
				};
			};
			object("Text")
			{
				recttransform(0.000, -0.500, 0.000, -20.000, -13.000)
				{
					anchor(0.000, 0.000, 1.000, 1.000);
					pivot(0.500, 0.500);
					rotation(0.000, 0.000, 0.000);
					scale(1.000, 1.000, 1.000);
					offset(10.000, 6.000, -10.000, -7.000);
				};
				component("Text", "UnityEngine.UI.Text, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
				{
					text("");
					color(0.196, 0.196, 0.196, 1.000);
					font("Arial", "Library/unity default resources", 14, 1, "Normal");
					align("UpperLeft", False);
				};
			};
		};
		object("PasswdLabel")
		{
			recttransform(-79.000, -7.000, 0.000, 50.000, 23.000)
			{
				anchor(0.500, 0.500, 0.500, 0.500);
				pivot(0.500, 0.500);
				rotation(0.000, 0.000, 0.000);
				scale(1.000, 1.000, 1.000);
				offset(-104.000, -18.500, -54.000, 4.500);
			};
			component("Text", "UnityEngine.UI.Text, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
			{
				text("密码：");
				color(0.196, 0.196, 0.196, 1.000);
				font("Arial", "Library/unity default resources", 14, 1, "Normal");
				align("UpperLeft", False);
			};
		};
		object("Passwd")
		{
			recttransform(26.000, -7.000, 0.000, 160.000, 30.000)
			{
				anchor(0.500, 0.500, 0.500, 0.500);
				pivot(0.500, 0.500);
				rotation(0.000, 0.000, 0.000);
				scale(1.000, 1.000, 1.000);
				offset(-54.000, -22.000, 106.000, 8.000);
			};
			component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
			{
				sprite("Resources/unity_builtin_extra");
				color(1.000, 1.000, 1.000, 1.000);
			};
			component("InputField", "UnityEngine.UI.InputField, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
			object("Placeholder")
			{
				recttransform(0.000, -0.500, 0.000, -20.000, -13.000)
				{
					anchor(0.000, 0.000, 1.000, 1.000);
					pivot(0.500, 0.500);
					rotation(0.000, 0.000, 0.000);
					scale(1.000, 1.000, 1.000);
					offset(10.000, 6.000, -10.000, -7.000);
				};
				component("Text", "UnityEngine.UI.Text, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
				{
					text("Enter text...");
					color(0.196, 0.196, 0.196, 0.500);
					font("Arial", "Library/unity default resources", 14, 1, "Italic");
					align("UpperLeft", False);
				};
			};
			object("Text")
			{
				recttransform(0.000, -0.500, 0.000, -20.000, -13.000)
				{
					anchor(0.000, 0.000, 1.000, 1.000);
					pivot(0.500, 0.500);
					rotation(0.000, 0.000, 0.000);
					scale(1.000, 1.000, 1.000);
					offset(10.000, 6.000, -10.000, -7.000);
				};
				component("Text", "UnityEngine.UI.Text, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
				{
					text("");
					color(0.196, 0.196, 0.196, 1.000);
					font("Arial", "Library/unity default resources", 14, 1, "Normal");
					align("UpperLeft", False);
				};
			};
		};
		object("Button")
		{
			recttransform(7.000, -47.000, 0.000, 101.000, 30.000)
			{
				anchor(0.500, 0.500, 0.500, 0.500);
				pivot(0.500, 0.500);
				rotation(0.000, 0.000, 0.000);
				scale(1.000, 1.000, 1.000);
				offset(-43.500, -62.000, 57.500, -32.000);
			};
			component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
			{
				sprite("Assets/UI/UITexture/FrameSmallBlue.png");
				color(1.000, 1.000, 1.000, 1.000);
			};
			component("Button", "UnityEngine.UI.Button, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
			object("Text")
			{
				recttransform(0.000, 0.000, 0.000, 0.000, 0.000)
				{
					anchor(0.000, 0.000, 1.000, 1.000);
					pivot(0.500, 0.500);
					rotation(0.000, 0.000, 0.000);
					scale(1.000, 1.000, 1.000);
					offset(0.000, 0.000, 0.000, 0.000);
				};
				component("Text", "UnityEngine.UI.Text, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
				{
					text("进入游戏");
					color(0.196, 0.196, 0.196, 1.000);
					font("Arial", "Library/unity default resources", 14, 1, "Normal");
					align("MiddleCenter", False);
				};
			};
		};
	};
};
object("MainUI")
{
	recttransform(0.000, 0.000, 0.000, 0.000, 0.000)
	{
		anchor(0.000, 0.000, 0.000, 0.000);
		pivot(0.000, 0.000);
		rotation(0.000, 0.000, 0.000);
		scale(0.000, 0.000, 0.000);
		offset(0.000, 0.000, 0.000, 0.000);
	};
	component("CanvasScaler", "UnityEngine.UI.CanvasScaler, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
	component("GraphicRaycaster", "UnityEngine.UI.GraphicRaycaster, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
	object("Panel")
	{
		recttransform(0.000, 0.000, 0.000, 0.000, 155.000)
		{
			anchor(0.000, 0.000, 1.000, 0.000);
			pivot(0.500, 0.000);
			rotation(0.000, 0.000, 0.000);
			scale(1.000, 1.000, 1.000);
			offset(0.000, 0.000, 0.000, 155.000);
		};
		component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
		{
			sprite("Assets/UI/UITexture/button.psd");
			color(1.000, 1.000, 1.000, 0.392);
		};
		object("Image")
		{
			recttransform(13.000, 10.000, 0.000, 140.000, 135.000)
			{
				anchor(0.000, 0.000, 0.000, 0.000);
				pivot(0.000, 0.000);
				rotation(0.000, 0.000, 0.000);
				scale(1.000, 1.000, 1.000);
				offset(13.000, 10.000, 153.000, 145.000);
			};
			component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
			{
				sprite("Assets/UI/UITexture/button.psd");
				color(1.000, 1.000, 1.000, 1.000);
			};
			object("RawImage")
			{
				recttransform(0.000, -1.000, 0.000, 119.000, 118.000)
				{
					anchor(0.500, 0.500, 0.500, 0.500);
					pivot(0.500, 0.500);
					rotation(0.000, 0.000, 0.000);
					scale(1.000, 1.000, 1.000);
					offset(-59.500, -60.000, 59.500, 58.000);
				};
				component("RawImage", "UnityEngine.UI.RawImage, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
				object("Player")
				{
					recttransform(-50.000, -50.000, 0.000, 10.000, 10.000)
					{
						anchor(0.500, 0.500, 0.500, 0.500);
						pivot(0.500, 0.500);
						rotation(0.000, 0.000, 0.000);
						scale(1.000, 1.000, 1.000);
						offset(-55.000, -55.000, -45.000, -45.000);
					};
					component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
					{
						sprite("Assets/UI/UITexture/jinbi.png");
						color(1.000, 1.000, 1.000, 1.000);
					};
				};
			};
		};
		object("Button1")
		{
			recttransform(-79.000, 79.000, 0.000, 57.000, 51.000)
			{
				anchor(1.000, 0.000, 1.000, 0.000);
				pivot(1.000, 0.000);
				rotation(0.000, 0.000, 0.000);
				scale(1.000, 1.000, 1.000);
				offset(-136.000, 79.000, -79.000, 130.000);
			};
			component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
			{
				sprite("Resources/unity_builtin_extra");
				color(0.384, 0.337, 0.310, 1.000);
			};
			component("Button", "UnityEngine.UI.Button, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
			object("Text")
			{
				recttransform(0.000, 0.000, 0.000, 0.000, 0.000)
				{
					anchor(0.000, 0.000, 1.000, 1.000);
					pivot(0.500, 0.500);
					rotation(0.000, 0.000, 0.000);
					scale(1.000, 1.000, 1.000);
					offset(0.000, 0.000, 0.000, 0.000);
				};
				component("Text", "UnityEngine.UI.Text, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
				{
					text("Skill1");
					color(0.196, 0.196, 0.196, 1.000);
					font("Arial", "Library/unity default resources", 14, 1, "Normal");
					align("MiddleCenter", False);
				};
			};
		};
		object("Button2")
		{
			recttransform(-27.000, 79.000, 0.000, 52.000, 51.000)
			{
				anchor(1.000, 0.000, 1.000, 0.000);
				pivot(1.000, 0.000);
				rotation(0.000, 0.000, 0.000);
				scale(1.000, 1.000, 1.000);
				offset(-79.000, 79.000, -27.000, 130.000);
			};
			component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
			{
				sprite("Resources/unity_builtin_extra");
				color(0.376, 0.337, 0.314, 1.000);
			};
			component("Button", "UnityEngine.UI.Button, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
			object("Text")
			{
				recttransform(0.000, 0.000, 0.000, 0.000, 0.000)
				{
					anchor(0.000, 0.000, 1.000, 1.000);
					pivot(0.500, 0.500);
					rotation(0.000, 0.000, 0.000);
					scale(1.000, 1.000, 1.000);
					offset(0.000, 0.000, 0.000, 0.000);
				};
				component("Text", "UnityEngine.UI.Text, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
				{
					text("Skill2");
					color(0.196, 0.196, 0.196, 1.000);
					font("Arial", "Library/unity default resources", 14, 1, "Normal");
					align("MiddleCenter", False);
				};
			};
		};
		object("Button3")
		{
			recttransform(-79.000, 29.000, 0.000, 56.000, 50.000)
			{
				anchor(1.000, 0.000, 1.000, 0.000);
				pivot(1.000, 0.000);
				rotation(0.000, 0.000, 0.000);
				scale(1.000, 1.000, 1.000);
				offset(-135.000, 29.000, -79.000, 79.000);
			};
			component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
			{
				sprite("Resources/unity_builtin_extra");
				color(0.376, 0.337, 0.314, 1.000);
			};
			component("Button", "UnityEngine.UI.Button, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
			object("Text")
			{
				recttransform(0.000, 0.000, 0.000, 0.000, 0.000)
				{
					anchor(0.000, 0.000, 1.000, 1.000);
					pivot(0.500, 0.500);
					rotation(0.000, 0.000, 0.000);
					scale(1.000, 1.000, 1.000);
					offset(0.000, 0.000, 0.000, 0.000);
				};
				component("Text", "UnityEngine.UI.Text, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
				{
					text("Skill3");
					color(0.196, 0.196, 0.196, 1.000);
					font("Arial", "Library/unity default resources", 14, 1, "Normal");
					align("MiddleCenter", False);
				};
			};
		};
		object("Button4")
		{
			recttransform(-27.000, 29.000, 0.000, 52.000, 50.000)
			{
				anchor(1.000, 0.000, 1.000, 0.000);
				pivot(1.000, 0.000);
				rotation(0.000, 0.000, 0.000);
				scale(1.000, 1.000, 1.000);
				offset(-79.000, 29.000, -27.000, 79.000);
			};
			component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
			{
				sprite("Resources/unity_builtin_extra");
				color(0.361, 0.325, 0.302, 1.000);
			};
			component("Button", "UnityEngine.UI.Button, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
			object("Text")
			{
				recttransform(0.000, 0.000, 0.000, 0.000, 0.000)
				{
					anchor(0.000, 0.000, 1.000, 1.000);
					pivot(0.500, 0.500);
					rotation(0.000, 0.000, 0.000);
					scale(1.000, 1.000, 1.000);
					offset(0.000, 0.000, 0.000, 0.000);
				};
				component("Text", "UnityEngine.UI.Text, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
				{
					text("Skill4");
					color(0.196, 0.196, 0.196, 1.000);
					font("Arial", "Library/unity default resources", 14, 1, "Normal");
					align("MiddleCenter", False);
				};
			};
		};
		object("TextBkg")
		{
			recttransform(2.000, 0.000, 0.000, 704.000, 155.000)
			{
				anchor(0.500, 0.500, 0.500, 0.500);
				pivot(0.500, 0.500);
				rotation(0.000, 0.000, 0.000);
				scale(1.000, 1.000, 1.000);
				offset(-350.000, -77.500, 354.000, 77.500);
			};
			component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
			{
				sprite("Assets/UI/UITexture/button.psd");
				color(0.420, 0.388, 0.357, 1.000);
			};
			object("ScrollView")
			{
				recttransform(3.000, 3.000, 0.000, -10.000, -14.000)
				{
					anchor(0.000, 0.000, 1.000, 1.000);
					pivot(0.500, 0.500);
					rotation(0.000, 0.000, 0.000);
					scale(1.000, 1.000, 1.000);
					offset(8.000, 10.000, -2.000, -4.000);
				};
				component("ScrollRect", "UnityEngine.UI.ScrollRect, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
				component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
				{
					sprite("");
					color(0.714, 0.714, 0.714, 0.000);
				};
				object("Viewport")
				{
					recttransform(0.000, 0.000, 0.000, 0.000, 0.000)
					{
						anchor(0.000, 0.000, 0.000, 0.000);
						pivot(0.000, 1.000);
						rotation(0.000, 0.000, 0.000);
						scale(1.000, 1.000, 1.000);
						offset(0.000, 0.000, 0.000, 0.000);
					};
					component("Mask", "UnityEngine.UI.Mask, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
					component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
					{
						sprite("");
						color(0.737, 0.737, 0.737, 1.000);
					};
					object("Content")
					{
						recttransform(0.000, 0.000, 0.000, 0.000, 4096.000)
						{
							anchor(0.000, 1.000, 1.000, 1.000);
							pivot(0.000, 1.000);
							rotation(0.000, 0.000, 0.000);
							scale(1.000, 1.000, 1.000);
							offset(0.000, -4096.000, 0.000, 0.000);
						};
					};
				};
				object("Scrollbar Horizontal")
				{
					recttransform(0.000, 0.000, 0.000, 0.000, 20.000)
					{
						anchor(0.000, 0.000, 0.000, 0.000);
						pivot(0.000, 0.000);
						rotation(0.000, 0.000, 0.000);
						scale(1.000, 1.000, 1.000);
						offset(0.000, 0.000, 0.000, 20.000);
					};
					component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
					{
						sprite("Resources/unity_builtin_extra");
						color(0.420, 0.388, 0.357, 1.000);
					};
					component("Scrollbar", "UnityEngine.UI.Scrollbar, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
					object("Sliding Area")
					{
						recttransform(0.000, 0.000, 0.000, -20.000, -20.000)
						{
							anchor(0.000, 0.000, 1.000, 1.000);
							pivot(0.500, 0.500);
							rotation(0.000, 0.000, 0.000);
							scale(1.000, 1.000, 1.000);
							offset(10.000, 10.000, -10.000, -10.000);
						};
						object("Handle")
						{
							recttransform(0.000, 0.000, 0.000, 20.000, 20.000)
							{
								anchor(0.000, 0.000, 0.000, 0.000);
								pivot(0.500, 0.500);
								rotation(0.000, 0.000, 0.000);
								scale(1.000, 1.000, 1.000);
								offset(-10.000, -10.000, 10.000, 10.000);
							};
							component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
							{
								sprite("Resources/unity_builtin_extra");
								color(1.000, 1.000, 1.000, 1.000);
							};
						};
					};
				};
				object("Scrollbar Vertical")
				{
					recttransform(0.000, 0.000, 0.000, 20.000, 0.000)
					{
						anchor(1.000, 0.000, 1.000, 0.000);
						pivot(1.000, 1.000);
						rotation(0.000, 0.000, 0.000);
						scale(1.000, 1.000, 1.000);
						offset(-20.000, 0.000, 0.000, 0.000);
					};
					component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
					{
						sprite("Resources/unity_builtin_extra");
						color(0.498, 0.463, 0.424, 1.000);
					};
					component("Scrollbar", "UnityEngine.UI.Scrollbar, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
					object("Sliding Area")
					{
						recttransform(0.000, 0.000, 0.000, -20.000, -20.000)
						{
							anchor(0.000, 0.000, 1.000, 1.000);
							pivot(0.500, 0.500);
							rotation(0.000, 0.000, 0.000);
							scale(1.000, 1.000, 1.000);
							offset(10.000, 10.000, -10.000, -10.000);
						};
						object("Handle")
						{
							recttransform(0.000, 0.000, 0.000, 20.000, 20.000)
							{
								anchor(0.000, 0.000, 0.000, 0.000);
								pivot(0.500, 0.500);
								rotation(0.000, 0.000, 0.000);
								scale(1.000, 1.000, 1.000);
								offset(-10.000, -10.000, 10.000, 10.000);
							};
							component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
							{
								sprite("Resources/unity_builtin_extra");
								color(1.000, 1.000, 1.000, 1.000);
							};
						};
					};
				};
			};
		};
	};
};
object("Nickname")
{
	recttransform(0.000, 0.000, 0.000, 0.000, 0.000)
	{
		anchor(0.000, 0.000, 0.000, 0.000);
		pivot(0.000, 0.000);
		rotation(0.000, 0.000, 0.000);
		scale(0.000, 0.000, 0.000);
		offset(0.000, 0.000, 0.000, 0.000);
	};
	component("CanvasScaler", "UnityEngine.UI.CanvasScaler, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
	component("GraphicRaycaster", "UnityEngine.UI.GraphicRaycaster, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
	object("Panel")
	{
		recttransform(-16.000, -12.000, 0.000, 346.000, 189.000)
		{
			anchor(0.500, 0.500, 0.500, 0.500);
			pivot(0.500, 0.500);
			rotation(0.000, 0.000, 0.000);
			scale(1.000, 1.000, 1.000);
			offset(-189.000, -106.500, 157.000, 82.500);
		};
		component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
		{
			sprite("Assets/UI/UITexture/Frame.png");
			color(1.000, 1.000, 1.000, 0.392);
		};
		object("NicknameLabel")
		{
			recttransform(-116.000, 15.000, 0.000, 43.800, 21.500)
			{
				anchor(0.500, 0.500, 0.500, 0.500);
				pivot(0.500, 0.500);
				rotation(0.000, 0.000, 0.000);
				scale(1.000, 1.000, 1.000);
				offset(-137.900, 4.250, -94.100, 25.750);
			};
			component("Text", "UnityEngine.UI.Text, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
			{
				text("名称：");
				color(0.196, 0.196, 0.196, 1.000);
				font("Arial", "Library/unity default resources", 14, 1, "Normal");
				align("UpperLeft", False);
			};
		};
		object("NicknameInput")
		{
			recttransform(-14.000, 15.000, 0.000, 160.000, 30.000)
			{
				anchor(0.500, 0.500, 0.500, 0.500);
				pivot(0.500, 0.500);
				rotation(0.000, 0.000, 0.000);
				scale(1.000, 1.000, 1.000);
				offset(-94.000, 0.000, 66.000, 30.000);
			};
			component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
			{
				sprite("Resources/unity_builtin_extra");
				color(1.000, 1.000, 1.000, 1.000);
			};
			component("InputField", "UnityEngine.UI.InputField, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
			object("Placeholder")
			{
				recttransform(0.000, -0.500, 0.000, -20.000, -13.000)
				{
					anchor(0.000, 0.000, 1.000, 1.000);
					pivot(0.500, 0.500);
					rotation(0.000, 0.000, 0.000);
					scale(1.000, 1.000, 1.000);
					offset(10.000, 6.000, -10.000, -7.000);
				};
				component("Text", "UnityEngine.UI.Text, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
				{
					text("Enter text...");
					color(0.196, 0.196, 0.196, 0.500);
					font("Arial", "Library/unity default resources", 14, 1, "Italic");
					align("UpperLeft", False);
				};
			};
			object("Text")
			{
				recttransform(0.000, -0.500, 0.000, -20.000, -13.000)
				{
					anchor(0.000, 0.000, 1.000, 1.000);
					pivot(0.500, 0.500);
					rotation(0.000, 0.000, 0.000);
					scale(1.000, 1.000, 1.000);
					offset(10.000, 6.000, -10.000, -7.000);
				};
				component("Text", "UnityEngine.UI.Text, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
				{
					text("");
					color(0.196, 0.196, 0.196, 1.000);
					font("Arial", "Library/unity default resources", 14, 1, "Normal");
					align("UpperLeft", False);
				};
			};
		};
		object("Roll")
		{
			recttransform(104.000, 15.000, 0.000, 76.000, 30.000)
			{
				anchor(0.500, 0.500, 0.500, 0.500);
				pivot(0.500, 0.500);
				rotation(0.000, 0.000, 0.000);
				scale(1.000, 1.000, 1.000);
				offset(66.000, 0.000, 142.000, 30.000);
			};
			component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
			{
				sprite("Assets/UI/UITexture/FrameSmallBlue.png");
				color(1.000, 1.000, 1.000, 1.000);
			};
			component("Button", "UnityEngine.UI.Button, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
			object("Text")
			{
				recttransform(0.000, 0.000, 0.000, 0.000, 0.000)
				{
					anchor(0.000, 0.000, 1.000, 1.000);
					pivot(0.500, 0.500);
					rotation(0.000, 0.000, 0.000);
					scale(1.000, 1.000, 1.000);
					offset(0.000, 0.000, 0.000, 0.000);
				};
				component("Text", "UnityEngine.UI.Text, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
				{
					text("换一个");
					color(0.196, 0.196, 0.196, 1.000);
					font("Arial", "Library/unity default resources", 14, 1, "Normal");
					align("MiddleCenter", False);
				};
			};
		};
		object("Button")
		{
			recttransform(-9.000, -23.000, 0.000, 54.200, 30.000)
			{
				anchor(0.500, 0.500, 0.500, 0.500);
				pivot(0.500, 0.500);
				rotation(0.000, 0.000, 0.000);
				scale(1.000, 1.000, 1.000);
				offset(-36.100, -38.000, 18.100, -8.000);
			};
			component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
			{
				sprite("Assets/UI/UITexture/FrameSmallRed.png");
				color(1.000, 1.000, 1.000, 1.000);
			};
			component("Button", "UnityEngine.UI.Button, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
			object("Text")
			{
				recttransform(0.000, 0.000, 0.000, 0.000, 0.000)
				{
					anchor(0.000, 0.000, 1.000, 1.000);
					pivot(0.500, 0.500);
					rotation(0.000, 0.000, 0.000);
					scale(1.000, 1.000, 1.000);
					offset(0.000, 0.000, 0.000, 0.000);
				};
				component("Text", "UnityEngine.UI.Text, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
				{
					text("确定
");
					color(0.196, 0.196, 0.196, 1.000);
					font("Arial", "Library/unity default resources", 14, 1, "Normal");
					align("MiddleCenter", False);
				};
			};
		};
	};
};
object("ScreenInput")
{
	recttransform(0.000, 0.000, 0.000, 0.000, 0.000)
	{
		anchor(0.000, 0.000, 1.000, 1.000);
		pivot(0.500, 0.500);
		rotation(0.000, 0.000, 0.000);
		scale(1.000, 1.000, 1.000);
		offset(0.000, 0.000, 0.000, 0.000);
	};
	object("MainPanel")
	{
		recttransform(0.000, 0.000, 0.000, 0.000, 0.000)
		{
			anchor(0.000, 0.000, 1.000, 1.000);
			pivot(0.500, 0.500);
			rotation(0.000, 0.000, 0.000);
			scale(1.000, 1.000, 1.000);
			offset(0.000, 0.000, 0.000, 0.000);
		};
		component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
		{
			sprite("");
			color(0.000, 0.000, 0.000, 0.000);
		};
		object("PanelAxis")
		{
			recttransform(516.000, 320.000, 0.000, 100.000, 100.000)
			{
				anchor(0.000, 0.000, 0.000, 0.000);
				pivot(0.500, 0.500);
				rotation(0.000, 0.000, 0.000);
				scale(1.000, 1.000, 1.000);
				offset(466.000, 270.000, 566.000, 370.000);
			};
			object("Background")
			{
				recttransform(0.000, 0.000, 0.000, 141.000, 147.400)
				{
					anchor(0.500, 0.500, 0.500, 0.500);
					pivot(0.500, 0.500);
					rotation(0.000, 0.000, 0.000);
					scale(1.000, 1.000, 1.000);
					offset(-70.500, -73.700, 70.500, 73.700);
				};
				component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
				{
					sprite("Assets/UI/UITexture/yaogan.png");
					color(1.000, 1.000, 1.000, 1.000);
				};
			};
			object("Button")
			{
				recttransform(0.000, 0.000, 0.000, 71.800, 73.800)
				{
					anchor(0.500, 0.500, 0.500, 0.500);
					pivot(0.500, 0.500);
					rotation(0.000, 0.000, 0.000);
					scale(1.000, 1.000, 1.000);
					offset(-35.900, -36.900, 35.900, 36.900);
				};
				component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
				{
					sprite("Assets/UI/UITexture/yaogan_point.png");
					color(1.000, 1.000, 1.000, 1.000);
				};
			};
		};
		object("PanelAxis2")
		{
			recttransform(-301.000, -34.000, 0.000, 100.000, 100.000)
			{
				anchor(0.500, 0.500, 0.500, 0.500);
				pivot(0.500, 0.500);
				rotation(0.000, 0.000, 0.000);
				scale(1.000, 1.000, 1.000);
				offset(-351.000, -84.000, -251.000, 16.000);
			};
			object("Head")
			{
				transform
				{
					position(0.000, 0.000, 0.000);
					rotation(0.000, 0.000, 0.000);
					scale(100.000, 100.000, 0.000);
				};
				component("MeshFilter", "UnityEngine.MeshFilter, UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null")
				{
				};
				component("MeshRenderer", "UnityEngine.MeshRenderer, UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null")
				{
				};
			};
			object("JoystickTail")
			{
				recttransform(0.000, 0.000, 0.000, 100.000, 100.000)
				{
					anchor(0.500, 0.500, 0.500, 0.500);
					pivot(0.500, 0.500);
					rotation(0.000, 0.000, 0.000);
					scale(9.000, 1.000, 1.000);
					offset(-50.000, -50.000, 50.000, 50.000);
				};
				object("Tail")
				{
					transform
					{
						position(0.000, 0.000, 0.000);
						rotation(0.000, 0.000, 0.000);
						scale(100.000, 100.000, 0.000);
					};
					component("MeshFilter", "UnityEngine.MeshFilter, UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null")
					{
					};
					component("MeshRenderer", "UnityEngine.MeshRenderer, UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null")
					{
					};
				};
			};
		};
		object("TouchEffect")
		{
			recttransform(172.000, 0.000, 0.000, 100.000, 100.000)
			{
				anchor(0.500, 0.500, 0.500, 0.500);
				pivot(0.500, 0.500);
				rotation(0.000, 0.000, 0.000);
				scale(1.000, 1.000, 1.000);
				offset(122.000, -50.000, 222.000, 50.000);
			};
			component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
			{
				sprite("Assets/UI/UITexture/joystick.png");
				color(1.000, 1.000, 1.000, 1.000);
			};
		};
	};
};
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
object("SkillViewerButtons")
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
		recttransform(0.000, 262.000, 0.000, 0.000, -524.000)
		{
			anchor(0.000, 0.000, 1.000, 1.000);
			pivot(0.500, 0.500);
			rotation(0.000, 0.000, 0.000);
			scale(1.000, 1.000, 1.000);
			offset(0.000, 524.000, 0.000, 0.000);
		};
		component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
		{
			sprite("Assets/Resources/UITexture/ActorBigIcon.png");
			color(1.000, 1.000, 1.000, 0.392);
		};
		object("Reload")
		{
			recttransform(-92.000, -2.000, 0.000, 146.000, 62.000)
			{
				anchor(0.500, 0.500, 0.500, 0.500);
				pivot(0.500, 0.500);
				rotation(0.000, 0.000, 0.000);
				scale(1.000, 1.000, 1.000);
				offset(-165.000, -33.000, -19.000, 29.000);
			};
			component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
			{
				sprite("Assets/UI/UITexture/FrameSmallBlue.png");
				color(1.000, 1.000, 1.000, 1.000);
			};
			component("Button", "UnityEngine.UI.Button, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
			object("Text")
			{
				recttransform(0.000, 0.000, 0.000, 0.000, 0.000)
				{
					anchor(0.000, 0.000, 1.000, 1.000);
					pivot(0.500, 0.500);
					rotation(0.000, 0.000, 0.000);
					scale(1.000, 1.000, 1.000);
					offset(0.000, 0.000, 0.000, 0.000);
				};
				component("Text", "UnityEngine.UI.Text, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
				{
					text("从文件加载
英雄与技能");
					color(1.000, 1.000, 1.000, 1.000);
					font("Arial", "Library/unity default resources", 18, 1, "Bold");
					align("MiddleCenter", False);
				};
			};
		};
		object("Review")
		{
			recttransform(210.000, -2.000, 0.000, 154.000, 62.000)
			{
				anchor(0.500, 0.500, 0.500, 0.500);
				pivot(0.500, 0.500);
				rotation(0.000, 0.000, 0.000);
				scale(1.000, 1.000, 1.000);
				offset(133.000, -33.000, 287.000, 29.000);
			};
			component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
			{
				sprite("Assets/UI/UITexture/FrameSmallBlue.png");
				color(1.000, 1.000, 1.000, 1.000);
			};
			component("Button", "UnityEngine.UI.Button, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
			object("Text")
			{
				recttransform(0.000, 0.000, 0.000, 0.000, 0.000)
				{
					anchor(0.000, 0.000, 1.000, 1.000);
					pivot(0.500, 0.500);
					rotation(0.000, 0.000, 0.000);
					scale(1.000, 1.000, 1.000);
					offset(0.000, 0.000, 0.000, 0.000);
				};
				component("Text", "UnityEngine.UI.Text, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
				{
					text("加载编辑过的
英雄与技能");
					color(1.000, 1.000, 1.000, 1.000);
					font("Arial", "Library/unity default resources", 18, 1, "Bold");
					align("MiddleCenter", False);
				};
			};
		};
		object("Actor")
		{
			recttransform(-216.000, -1.000, 0.000, 97.000, 46.000)
			{
				anchor(0.500, 0.500, 0.500, 0.500);
				pivot(0.500, 0.500);
				rotation(0.000, 0.000, 0.000);
				scale(1.000, 1.000, 1.000);
				offset(-264.500, -24.000, -167.500, 22.000);
			};
			component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
			{
				sprite("Resources/unity_builtin_extra");
				color(1.000, 1.000, 1.000, 1.000);
			};
			component("InputField", "UnityEngine.UI.InputField, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
			object("Placeholder")
			{
				recttransform(0.000, -0.500, 0.000, -20.000, -13.000)
				{
					anchor(0.000, 0.000, 1.000, 1.000);
					pivot(0.500, 0.500);
					rotation(0.000, 0.000, 0.000);
					scale(1.000, 1.000, 1.000);
					offset(10.000, 6.000, -10.000, -7.000);
				};
				component("Text", "UnityEngine.UI.Text, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
				{
					text("Enter text...");
					color(0.196, 0.196, 0.196, 0.500);
					font("Arial", "Library/unity default resources", 14, 1, "Italic");
					align("UpperLeft", False);
				};
			};
			object("Text")
			{
				recttransform(0.000, -0.500, 0.000, -20.000, -13.000)
				{
					anchor(0.000, 0.000, 1.000, 1.000);
					pivot(0.500, 0.500);
					rotation(0.000, 0.000, 0.000);
					scale(1.000, 1.000, 1.000);
					offset(10.000, 6.000, -10.000, -7.000);
				};
				component("Text", "UnityEngine.UI.Text, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
				{
					text("101");
					color(0.196, 0.196, 0.196, 1.000);
					font("Arial", "Library/unity default resources", 24, 1, "Normal");
					align("MiddleLeft", False);
				};
			};
		};
		object("Text")
		{
			recttransform(-300.000, -1.000, 0.000, 72.000, 46.000)
			{
				anchor(0.500, 0.500, 0.500, 0.500);
				pivot(0.500, 0.500);
				rotation(0.000, 0.000, 0.000);
				scale(1.000, 1.000, 1.000);
				offset(-336.000, -24.000, -264.000, 22.000);
			};
			component("Text", "UnityEngine.UI.Text, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
			{
				text("角色ID：");
				color(1.000, 1.000, 1.000, 1.000);
				font("Arial", "Library/unity default resources", 18, 1, "Normal");
				align("MiddleLeft", False);
			};
		};
		object("CopyToClipboard")
		{
			recttransform(354.000, -2.000, 0.000, 134.000, 62.000)
			{
				anchor(0.500, 0.500, 0.500, 0.500);
				pivot(0.500, 0.500);
				rotation(0.000, 0.000, 0.000);
				scale(1.000, 1.000, 1.000);
				offset(287.000, -33.000, 421.000, 29.000);
			};
			component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
			{
				sprite("Assets/UI/UITexture/FrameSmallRed.png");
				color(1.000, 1.000, 1.000, 1.000);
			};
			component("Button", "UnityEngine.UI.Button, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
			object("Text")
			{
				recttransform(0.000, 0.000, 0.000, 0.000, 0.000)
				{
					anchor(0.000, 0.000, 1.000, 1.000);
					pivot(0.500, 0.500);
					rotation(0.000, 0.000, 0.000);
					scale(1.000, 1.000, 1.000);
					offset(0.000, 0.000, 0.000, 0.000);
				};
				component("Text", "UnityEngine.UI.Text, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
				{
					text("编辑数据 -> 剪贴板");
					color(1.000, 1.000, 1.000, 1.000);
					font("Arial", "Library/unity default resources", 18, 1, "Normal");
					align("MiddleCenter", False);
				};
			};
		};
		object("NewHeroSkills")
		{
			recttransform(57.000, -2.000, 0.000, 152.000, 62.000)
			{
				anchor(0.500, 0.500, 0.500, 0.500);
				pivot(0.500, 0.500);
				rotation(0.000, 0.000, 0.000);
				scale(1.000, 1.000, 1.000);
				offset(-19.000, -33.000, 133.000, 29.000);
			};
			component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
			{
				sprite("Assets/UI/UITexture/FrameSmallBlue.png");
				color(1.000, 1.000, 1.000, 1.000);
			};
			component("Button", "UnityEngine.UI.Button, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
			object("Text")
			{
				recttransform(0.000, 0.000, 0.000, 0.000, 0.000)
				{
					anchor(0.000, 0.000, 1.000, 1.000);
					pivot(0.500, 0.500);
					rotation(0.000, 0.000, 0.000);
					scale(1.000, 1.000, 1.000);
					offset(0.000, 0.000, 0.000, 0.000);
				};
				component("Text", "UnityEngine.UI.Text, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
				{
					text("加载或创建
英雄技能数据");
					color(1.000, 1.000, 1.000, 1.000);
					font("Arial", "Library/unity default resources", 18, 1, "Normal");
					align("MiddleCenter", False);
				};
			};
		};
		object("Text2")
		{
			recttransform(-490.000, -2.000, 0.000, 111.000, 46.000)
			{
				anchor(0.500, 0.500, 0.500, 0.500);
				pivot(0.500, 0.500);
				rotation(0.000, 0.000, 0.000);
				scale(1.000, 1.000, 1.000);
				offset(-545.500, -25.000, -434.500, 21.000);
			};
			component("Text", "UnityEngine.UI.Text, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
			{
				text("受击角色ID：");
				color(1.000, 1.000, 1.000, 1.000);
				font("Arial", "Library/unity default resources", 18, 1, "Normal");
				align("MiddleLeft", False);
			};
		};
		object("Actor2")
		{
			recttransform(-391.000, -2.000, 0.000, 88.000, 46.000)
			{
				anchor(0.500, 0.500, 0.500, 0.500);
				pivot(0.500, 0.500);
				rotation(0.000, 0.000, 0.000);
				scale(1.000, 1.000, 1.000);
				offset(-435.000, -25.000, -347.000, 21.000);
			};
			component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
			{
				sprite("Resources/unity_builtin_extra");
				color(1.000, 1.000, 1.000, 1.000);
			};
			component("InputField", "UnityEngine.UI.InputField, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
			object("Placeholder")
			{
				recttransform(0.000, -0.500, 0.000, -20.000, -13.000)
				{
					anchor(0.000, 0.000, 1.000, 1.000);
					pivot(0.500, 0.500);
					rotation(0.000, 0.000, 0.000);
					scale(1.000, 1.000, 1.000);
					offset(10.000, 6.000, -10.000, -7.000);
				};
				component("Text", "UnityEngine.UI.Text, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
				{
					text("Enter text...");
					color(0.196, 0.196, 0.196, 0.500);
					font("Arial", "Library/unity default resources", 14, 1, "Italic");
					align("UpperLeft", False);
				};
			};
			object("Text")
			{
				recttransform(0.000, -0.500, 0.000, -20.000, -13.000)
				{
					anchor(0.000, 0.000, 1.000, 1.000);
					pivot(0.500, 0.500);
					rotation(0.000, 0.000, 0.000);
					scale(1.000, 1.000, 1.000);
					offset(10.000, 6.000, -10.000, -7.000);
				};
				component("Text", "UnityEngine.UI.Text, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
				{
					text("1");
					color(0.196, 0.196, 0.196, 1.000);
					font("Arial", "Library/unity default resources", 24, 1, "Normal");
					align("MiddleLeft", False);
				};
			};
		};
		object("SingleStep")
		{
			recttransform(485.000, 1.000, 0.000, 90.000, 36.000)
			{
				anchor(0.500, 0.500, 0.500, 0.500);
				pivot(0.500, 0.500);
				rotation(0.000, 0.000, 0.000);
				scale(1.000, 1.000, 1.000);
				offset(440.000, -17.000, 530.000, 19.000);
			};
			component("Toggle", "UnityEngine.UI.Toggle, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
			object("Background")
			{
				recttransform(10.000, -10.000, 0.000, 20.000, 20.000)
				{
					anchor(0.000, 1.000, 0.000, 1.000);
					pivot(0.500, 0.500);
					rotation(0.000, 0.000, 0.000);
					scale(1.000, 1.000, 1.000);
					offset(0.000, -20.000, 20.000, 0.000);
				};
				component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
				{
					sprite("Resources/unity_builtin_extra");
					color(1.000, 1.000, 1.000, 1.000);
				};
				object("Checkmark")
				{
					recttransform(0.000, 0.000, 0.000, 20.000, 20.000)
					{
						anchor(0.500, 0.500, 0.500, 0.500);
						pivot(0.500, 0.500);
						rotation(0.000, 0.000, 0.000);
						scale(1.000, 1.000, 1.000);
						offset(-10.000, -10.000, 10.000, 10.000);
					};
					component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
					{
						sprite("Resources/unity_builtin_extra");
						color(1.000, 1.000, 1.000, 1.000);
					};
				};
			};
			object("Label")
			{
				recttransform(9.000, -0.500, 0.000, -28.000, -3.000)
				{
					anchor(0.000, 0.000, 1.000, 1.000);
					pivot(0.500, 0.500);
					rotation(0.000, 0.000, 0.000);
					scale(1.000, 1.000, 1.000);
					offset(23.000, 1.000, -5.000, -2.000);
				};
				component("Text", "UnityEngine.UI.Text, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
				{
					text("单步");
					color(1.000, 1.000, 1.000, 1.000);
					font("Arial", "Library/unity default resources", 24, 1, "Normal");
					align("MiddleCenter", False);
				};
			};
		};
	};
};
object("StoryDlg")
{
	recttransform(0.000, 0.000, 0.000, 0.000, 0.000)
	{
		anchor(0.000, 0.000, 0.000, 0.000);
		pivot(0.000, 0.000);
		rotation(0.000, 0.000, 0.000);
		scale(0.000, 0.000, 0.000);
		offset(0.000, 0.000, 0.000, 0.000);
	};
	component("CanvasScaler", "UnityEngine.UI.CanvasScaler, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
	component("GraphicRaycaster", "UnityEngine.UI.GraphicRaycaster, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
	object("Panel")
	{
		recttransform(0.000, 0.000, 0.000, 0.000, 0.000)
		{
			anchor(0.000, 0.000, 1.000, 1.000);
			pivot(0.500, 0.500);
			rotation(0.000, 0.000, 0.000);
			scale(1.000, 1.000, 1.000);
			offset(0.000, 0.000, 0.000, 0.000);
		};
		component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
		{
			sprite("Assets/Resources/UITexture/SkillIcons.png");
			color(1.000, 1.000, 1.000, 1.000);
		};
		object("ScrollView")
		{
			recttransform(0.000, 1.000, 0.000, 894.000, 462.000)
			{
				anchor(0.500, 0.500, 0.500, 0.500);
				pivot(0.500, 0.500);
				rotation(0.000, 0.000, 0.000);
				scale(1.000, 1.000, 1.000);
				offset(-447.000, -230.000, 447.000, 232.000);
			};
			component("ScrollRect", "UnityEngine.UI.ScrollRect, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
			component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
			{
				sprite("Assets/Resources/UITexture/SkillIcons.png");
				color(0.000, 0.000, 0.000, 0.392);
			};
			object("Viewport")
			{
				recttransform(0.000, 0.000, 0.000, 0.000, 0.000)
				{
					anchor(0.000, 0.000, 0.000, 0.000);
					pivot(0.000, 1.000);
					rotation(0.000, 0.000, 0.000);
					scale(1.000, 1.000, 1.000);
					offset(0.000, 0.000, 0.000, 0.000);
				};
				component("Mask", "UnityEngine.UI.Mask, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
				component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
				{
					sprite("Assets/Resources/UITexture/SkillIcons.png");
					color(0.380, 0.380, 0.380, 1.000);
				};
				object("Content")
				{
					recttransform(0.000, 0.000, 0.000, 0.000, 4096.000)
					{
						anchor(0.000, 1.000, 1.000, 1.000);
						pivot(0.000, 1.000);
						rotation(0.000, 0.000, 0.000);
						scale(1.000, 1.000, 1.000);
						offset(0.000, -4096.000, 0.000, 0.000);
					};
				};
			};
			object("Scrollbar Horizontal")
			{
				recttransform(0.000, 0.000, 0.000, 0.000, 20.000)
				{
					anchor(0.000, 0.000, 0.000, 0.000);
					pivot(0.000, 0.000);
					rotation(0.000, 0.000, 0.000);
					scale(1.000, 1.000, 1.000);
					offset(0.000, 0.000, 0.000, 20.000);
				};
				component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
				{
					sprite("Assets/Resources/UITexture/SkillIcons.png");
					color(0.035, 0.047, 0.059, 1.000);
				};
				component("Scrollbar", "UnityEngine.UI.Scrollbar, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
				object("Sliding Area")
				{
					recttransform(0.000, 0.000, 0.000, -20.000, -20.000)
					{
						anchor(0.000, 0.000, 1.000, 1.000);
						pivot(0.500, 0.500);
						rotation(0.000, 0.000, 0.000);
						scale(1.000, 1.000, 1.000);
						offset(10.000, 10.000, -10.000, -10.000);
					};
					object("Handle")
					{
						recttransform(0.000, 0.000, 0.000, 20.000, 20.000)
						{
							anchor(0.000, 0.000, 0.000, 0.000);
							pivot(0.500, 0.500);
							rotation(0.000, 0.000, 0.000);
							scale(1.000, 1.000, 1.000);
							offset(-10.000, -10.000, 10.000, 10.000);
						};
						component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
						{
							sprite("Resources/unity_builtin_extra");
							color(1.000, 1.000, 1.000, 1.000);
						};
					};
				};
			};
			object("Scrollbar Vertical")
			{
				recttransform(0.000, 0.000, 0.000, 20.000, 0.000)
				{
					anchor(1.000, 0.000, 1.000, 0.000);
					pivot(1.000, 1.000);
					rotation(0.000, 0.000, 0.000);
					scale(1.000, 1.000, 1.000);
					offset(-20.000, 0.000, 0.000, 0.000);
				};
				component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
				{
					sprite("Assets/Resources/UITexture/SkillIcons.png");
					color(0.745, 0.745, 0.745, 1.000);
				};
				component("Scrollbar", "UnityEngine.UI.Scrollbar, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
				object("Sliding Area")
				{
					recttransform(0.000, 0.000, 0.000, -20.000, -20.000)
					{
						anchor(0.000, 0.000, 1.000, 1.000);
						pivot(0.500, 0.500);
						rotation(0.000, 0.000, 0.000);
						scale(1.000, 1.000, 1.000);
						offset(10.000, 10.000, -10.000, -10.000);
					};
					object("Handle")
					{
						recttransform(0.000, 0.000, 0.000, 20.000, 20.000)
						{
							anchor(0.000, 0.000, 0.000, 0.000);
							pivot(0.500, 0.500);
							rotation(0.000, 0.000, 0.000);
							scale(1.000, 1.000, 1.000);
							offset(-10.000, -10.000, 10.000, 10.000);
						};
						component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
						{
							sprite("Resources/unity_builtin_extra");
							color(1.000, 1.000, 1.000, 1.000);
						};
					};
				};
			};
		};
		object("Button")
		{
			recttransform(-156.000, 14.000, 0.000, 160.000, 30.000)
			{
				anchor(1.000, 0.000, 1.000, 0.000);
				pivot(1.000, 0.000);
				rotation(0.000, 0.000, 0.000);
				scale(1.000, 1.000, 1.000);
				offset(-316.000, 14.000, -156.000, 44.000);
			};
			component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
			{
				sprite("Resources/unity_builtin_extra");
				color(1.000, 1.000, 1.000, 1.000);
			};
			component("Button", "UnityEngine.UI.Button, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
			object("Text")
			{
				recttransform(0.000, 0.000, 0.000, 0.000, 0.000)
				{
					anchor(0.000, 0.000, 1.000, 1.000);
					pivot(0.500, 0.500);
					rotation(0.000, 0.000, 0.000);
					scale(1.000, 1.000, 1.000);
					offset(0.000, 0.000, 0.000, 0.000);
				};
				component("Text", "UnityEngine.UI.Text, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
				{
					text("Next");
					color(0.721, 0.988, 1.000, 1.000);
					font("Arial", "Library/unity default resources", 18, 1, "Normal");
					align("MiddleCenter", False);
				};
			};
		};
	};
};
object("SummonIconItem")
{
	recttransform(0.000, -0.800, 0.000, 80.000, 110.000)
	{
		anchor(0.500, 0.500, 0.500, 0.500);
		pivot(0.500, 0.500);
		rotation(0.000, 0.000, 0.000);
		scale(1.000, 1.000, 1.000);
		offset(-40.000, -55.800, 40.000, 54.200);
	};
	object("Image")
	{
		recttransform(-0.100, -40.000, 0.000, 70.000, 70.000)
		{
			anchor(0.500, 1.000, 0.500, 1.000);
			pivot(0.500, 0.500);
			rotation(0.000, 0.000, 0.000);
			scale(1.000, 1.000, 1.000);
			offset(-35.100, -75.000, 34.900, -5.000);
		};
		component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
		{
			sprite("Assets/Resources/UITexture/SkillIcons.png");
			color(1.000, 1.000, 1.000, 1.000);
		};
	};
	object("Button")
	{
		recttransform(-0.075, -40.000, 0.000, -0.050, 80.000)
		{
			anchor(0.000, 1.000, 1.000, 1.000);
			pivot(0.500, 0.500);
			rotation(0.000, 0.000, 0.000);
			scale(1.000, 1.000, 1.000);
			offset(-0.050, -80.000, -0.100, 0.000);
		};
		component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
		{
			sprite("Assets/UI/UITexture/t_duizhang.png");
			color(1.000, 1.000, 1.000, 1.000);
		};
		component("Button", "UnityEngine.UI.Button, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
		object("Cooldown")
		{
			recttransform(0.000, 0.000, 0.000, 0.000, 0.000)
			{
				anchor(0.000, 0.000, 1.000, 1.000);
				pivot(0.500, 0.500);
				rotation(0.000, 0.000, 0.000);
				scale(1.000, 1.000, 1.000);
				offset(0.000, 0.000, 0.000, 0.000);
			};
			component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
			{
				sprite("Assets/Resources/UITexture/SkillIcons.png");
				color(1.000, 1.000, 1.000, 0.397);
			};
		};
		object("ui_fang")
		{
			transform
			{
				position(0.000, 1.170, 0.000);
				rotation(0.000, 0.000, 0.000);
				scale(1.000, 1.000, 1.000);
			};
			component("ParticleSystem", "UnityEngine.ParticleSystem, UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null");
			component("ParticleSystemRenderer", "UnityEngine.ParticleSystemRenderer, UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null");
			object("Particle System6")
			{
				transform
				{
					position(0.000, 0.000, 0.000);
					rotation(0.000, 0.000, 0.000);
					scale(1.000, 1.000, 1.000);
				};
				component("ParticleSystem", "UnityEngine.ParticleSystem, UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null");
				component("ParticleSystemRenderer", "UnityEngine.ParticleSystemRenderer, UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null");
			};
		};
	};
	object("Panel")
	{
		recttransform(0.000, 21.950, 0.000, 0.000, 16.100)
		{
			anchor(0.000, 0.000, 1.000, 0.000);
			pivot(0.500, 0.500);
			rotation(0.000, 0.000, 0.000);
			scale(1.000, 1.000, 1.000);
			offset(0.000, 13.900, 0.000, 30.000);
		};
		component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
		{
			sprite("");
			color(1.000, 1.000, 1.000, 1.000);
		};
		object("SliderHp")
		{
			recttransform(-0.800, 11.800, 0.000, -6.300, 5.800)
			{
				anchor(0.000, 0.000, 1.000, 0.000);
				pivot(0.500, 0.500);
				rotation(0.000, 0.000, 0.000);
				scale(1.000, 1.000, 1.000);
				offset(2.350, 8.900, -3.950, 14.700);
			};
			component("Slider", "UnityEngine.UI.Slider, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
			object("Fill Area")
			{
				recttransform(0.000, 0.000, 0.000, 0.000, 0.000)
				{
					anchor(0.000, 0.000, 1.000, 1.000);
					pivot(0.500, 0.500);
					rotation(0.000, 0.000, 0.000);
					scale(1.000, 1.000, 1.000);
					offset(0.000, 0.000, 0.000, 0.000);
				};
				object("Fill")
				{
					recttransform(0.000, 0.000, 0.000, 0.000, 0.000)
					{
						anchor(0.000, 0.000, 0.000, 0.000);
						pivot(0.500, 0.500);
						rotation(0.000, 0.000, 0.000);
						scale(1.000, 1.000, 1.000);
						offset(0.000, 0.000, 0.000, 0.000);
					};
					component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
					{
						sprite("Assets/UI/UITexture/BloodRed.png");
						color(1.000, 1.000, 1.000, 1.000);
					};
				};
			};
		};
		object("SliderMp")
		{
			recttransform(-0.800, 4.600, 0.000, -6.300, 4.700)
			{
				anchor(0.000, 0.000, 1.000, 0.000);
				pivot(0.500, 0.500);
				rotation(0.000, 0.000, 0.000);
				scale(1.000, 1.000, 1.000);
				offset(2.350, 2.250, -3.950, 6.950);
			};
			component("Slider", "UnityEngine.UI.Slider, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
			object("Fill Area")
			{
				recttransform(-0.400, 0.000, 0.000, 0.800, 0.000)
				{
					anchor(0.000, 0.000, 1.000, 1.000);
					pivot(0.500, 0.500);
					rotation(0.000, 0.000, 0.000);
					scale(1.000, 1.000, 1.000);
					offset(-0.800, 0.000, 0.000, 0.000);
				};
				object("Fill")
				{
					recttransform(0.000, 0.000, 0.000, 0.000, 0.000)
					{
						anchor(0.000, 0.000, 0.000, 0.000);
						pivot(0.500, 0.500);
						rotation(0.000, 0.000, 0.000);
						scale(1.000, 1.000, 1.000);
						offset(0.000, 0.000, 0.000, 0.000);
					};
					component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
					{
						sprite("Assets/UI/UITexture/blood_skill.png");
						color(1.000, 1.000, 1.000, 1.000);
					};
				};
			};
		};
		object("MpChange")
		{
			recttransform(0.000, -8.900, 0.000, 96.000, 45.800)
			{
				anchor(0.500, 0.500, 0.500, 0.500);
				pivot(0.500, 0.000);
				rotation(0.000, 0.000, 0.000);
				scale(1.000, 1.000, 1.000);
				offset(-48.000, -8.900, 48.000, 36.900);
			};
			component("Text", "UnityEngine.UI.Text, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
			{
				text("50");
				color(0.000, 0.876, 1.000, 1.000);
				font("Arial", "Library/unity default resources", 40, 1, "Bold");
				align("LowerCenter", False);
			};
		};
	};
	object("DeadMask")
	{
		recttransform(-0.025, 1.050, 0.000, 0.050, -2.100)
		{
			anchor(0.000, 0.000, 1.000, 1.000);
			pivot(0.500, 0.500);
			rotation(0.000, 0.000, 0.000);
			scale(1.000, 1.000, 1.000);
			offset(-0.050, 2.100, 0.000, 0.000);
		};
		component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
		{
			sprite("");
			color(0.162, 0.162, 0.162, 0.834);
		};
	};
	object("DisableMask")
	{
		recttransform(0.000, 1.050, 0.000, 0.000, -2.100)
		{
			anchor(0.000, 0.000, 1.000, 1.000);
			pivot(0.500, 0.500);
			rotation(0.000, 0.000, 0.000);
			scale(1.000, 1.000, 1.000);
			offset(0.000, 2.100, 0.000, 0.000);
		};
		component("Image", "UnityEngine.UI.Image, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
		{
			sprite("");
			color(0.669, 0.669, 0.669, 0.509);
		};
	};
	object("Text")
	{
		recttransform(-0.075, 6.950, 0.000, 79.900, 13.900)
		{
			anchor(0.500, 0.000, 0.500, 0.000);
			pivot(0.500, 0.500);
			rotation(0.000, 0.000, 0.000);
			scale(1.000, 1.000, 1.000);
			offset(-40.025, 0.000, 39.875, 13.900);
		};
		component("Text", "UnityEngine.UI.Text, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
		{
			text("1001");
			color(1.000, 0.000, 0.000, 1.000);
			font("Arial", "Library/unity default resources", 18, 1, "Normal");
			align("MiddleCenter", False);
		};
	};
};
object("TopMenu")
{
	recttransform(0.000, 0.000, 0.000, 0.000, 0.000)
	{
		anchor(0.000, 0.000, 0.000, 0.000);
		pivot(0.000, 0.000);
		rotation(0.000, 0.000, 0.000);
		scale(0.000, 0.000, 0.000);
		offset(0.000, 0.000, 0.000, 0.000);
	};
	component("CanvasScaler", "UnityEngine.UI.CanvasScaler, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
	component("GraphicRaycaster", "UnityEngine.UI.GraphicRaycaster, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
	object("PanelMiddle")
	{
		recttransform(-1.000, -52.000, 0.000, 570.000, 104.000)
		{
			anchor(0.500, 1.000, 0.500, 1.000);
			pivot(0.500, 0.500);
			rotation(0.000, 0.000, 0.000);
			scale(1.000, 1.000, 1.000);
			offset(-286.000, -104.000, 284.000, 0.000);
		};
		object("Panel")
		{
			recttransform(0.000, 7.200, 0.000, -288.000, -37.000)
			{
				anchor(0.000, 0.000, 1.000, 1.000);
				pivot(0.500, 0.500);
				rotation(0.000, 0.000, 0.000);
				scale(0.750, 0.750, 1.000);
				offset(144.000, 25.700, -144.000, -11.300);
			};
			object("Text")
			{
				recttransform(3.500, -2.675, 0.000, 169.800, 61.650)
				{
					anchor(0.500, 0.500, 0.500, 0.500);
					pivot(0.500, 0.500);
					rotation(0.000, 0.000, 0.000);
					scale(1.000, 1.000, 1.000);
					offset(-81.400, -33.500, 88.400, 28.150);
				};
				component("Text", "UnityEngine.UI.Text, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
				{
					text("1:57");
					color(1.000, 1.000, 1.000, 1.000);
					font("daojishi", "Assets/UI/UITexture/daojishi.fontsettings", 0, 1, "Normal");
					align("MiddleCenter", False);
				};
			};
		};
	};
};
