namespace BF1ServerTools.Themes.Controls;

public class UiLabel : Label
{
    /// <summary>
    /// 标题
    /// </summary>
    public string Title
    {
        get { return (string)GetValue(TitleProperty); }
        set { SetValue(TitleProperty, value); }
    }
    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register("Title", typeof(string), typeof(UiLabel), new PropertyMetadata(default));

    /// <summary>
    /// 描述
    /// </summary>
    public string Description
    {
        get { return (string)GetValue(DescriptionProperty); }
        set { SetValue(DescriptionProperty, value); }
    }
    public static readonly DependencyProperty DescriptionProperty =
        DependencyProperty.Register("Description", typeof(string), typeof(UiLabel), new PropertyMetadata(default));
}
