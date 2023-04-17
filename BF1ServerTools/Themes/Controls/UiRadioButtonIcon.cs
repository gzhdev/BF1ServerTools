namespace BF1ServerTools.Themes.Controls;

public class UiRadioButtonIcon : RadioButton
{
    /// <summary>
    /// Icon图标
    /// </summary>
    public string Icon
    {
        get { return (string)GetValue(IconProperty); }
        set { SetValue(IconProperty, value); }
    }
    public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register("Icon", typeof(string), typeof(UiRadioButtonIcon), new PropertyMetadata("\xe63b"));

    /// <summary>
    /// 标题
    /// </summary>
    public string Title
    {
        get { return (string)GetValue(TitleProperty); }
        set { SetValue(TitleProperty, value); }
    }
    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register("Title", typeof(string), typeof(UiRadioButtonIcon), new PropertyMetadata(default));

    /// <summary>
    /// 描述信息
    /// </summary>
    public string Description
    {
        get { return (string)GetValue(DescriptionProperty); }
        set { SetValue(DescriptionProperty, value); }
    }
    public static readonly DependencyProperty DescriptionProperty =
        DependencyProperty.Register("Description", typeof(string), typeof(UiRadioButtonIcon), new PropertyMetadata(default));
}
