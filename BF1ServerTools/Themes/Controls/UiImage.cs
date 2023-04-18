using BF1ServerTools.Helpers;

namespace BF1ServerTools.Themes.Controls;

public class UiImage : Image
{
    /// <summary>
    /// 图片字符串链接
    /// </summary>
    public string ImageUrl
    {
        get { return (string)GetValue(ImageUrlProperty); }
        set { SetValue(ImageUrlProperty, value); }
    }
    public static readonly DependencyProperty ImageUrlProperty =
        DependencyProperty.Register("ImageUrl", typeof(string), typeof(UiImage), new PropertyMetadata(string.Empty, ImageUrlPropertyChanged));

    private static async void ImageUrlPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
        var imageObj = (Image)obj;
        var url = e.NewValue as string;

        // 判断图片路径是否为空
        if (string.IsNullOrWhiteSpace(url))
        {
            imageObj.Source = null;
            return;
        }

        // 处理其他图片
        if (!url.StartsWith("http"))
        {
            imageObj.Source = new BitmapImage(new Uri(url, UriKind.RelativeOrAbsolute));
            return;
        }

        // 处理网络图片
        var bytes = await HttpHelper.GetWebImageBytes(url);
        if (bytes == null)
            return;

        var bitmapImage = new BitmapImage
        {
            CreateOptions = BitmapCreateOptions.DelayCreation,
            CacheOption = BitmapCacheOption.OnLoad,
            DecodePixelWidth = 100
        };

        bitmapImage.BeginInit();
        bitmapImage.StreamSource = new MemoryStream(bytes);
        bitmapImage.EndInit();
        bitmapImage.Freeze();

        imageObj.Source = bitmapImage;
    }
}
