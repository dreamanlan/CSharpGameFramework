#if UNITY_EDITOR
using UnityEditor;


public partial class BRT_BuildReportWindow
{
	// GUI Settings
	public const string DEFAULT_GUI_SKIN_FILENAME = "BuildReportWindow.guiskin";
	public const string DARK_GUI_SKIN_FILENAME = "BuildReportWindowDark.guiskin";

	// list normal is a list style with normal font size
	const string LIST_NORMAL_STYLE_NAME = "ListNormal";
	const string LIST_NORMAL_ALT_STYLE_NAME = "ListAltNormal";

	// list small is a list style with smaller font size (for the asset list)
	const string LIST_SMALL_STYLE_NAME = "List";
	const string LIST_SMALL_ALT_STYLE_NAME = "ListAlt";
	const string LIST_SMALL_SELECTED_NAME = "ListAltSelected";

	const string MAIN_TITLE_STYLE_NAME = "Title";
	const string MAIN_SUBTITLE_STYLE_NAME = "Subtitle";
	const string TINY_HELP_STYLE_NAME = "TinyHelp";

	const string INFO_TITLE_STYLE_NAME = "Big1";

	const string INFO_SUBTITLE_STYLE_NAME = "Header2";
	const string INFO_SUBTITLE_BOLD_STYLE_NAME = "Header2Bold";

	const string BIG_NUMBER_STYLE_NAME = "Big2";

	const float CATEGORY_VERTICAL_SPACING = 40;
	const float CATEGORY_HORIZONTAL_SPACING = 30;
}

#endif
