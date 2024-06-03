namespace Faile;

public partial class Fail_Page : ContentPage
{
    string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    public Fail_Page()
    {
        InitializeComponent();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        UpdateFilesList();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        string fileName = fileNameEntry.Text;
        if (String.IsNullOrEmpty(fileName)) { return; }
        if (File.Exists(Path.Combine(folderPath, fileName)))
        {
            bool isRewrited = await DisplayAlert("Kinitus", "Fail juba olemas. Kas tahas ümber kirjutada?", "jah", "ei");
            if (isRewrited == false) { return; }
        }
        File.WriteAllText(Path.Combine(folderPath, fileName), textEditor.Text);
        UpdateFilesList();
    }
    private void UpdateFilesList()
    {
        FilesList.ItemsSource = Directory.GetFiles(folderPath).Select(f => Path.GetFileName(f));
        FilesList.SelectedItem = null;
    }
    private void FilesList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem == null)
        {
            return;
        }
        string fileName = e.SelectedItem.ToString();
        textEditor.Text = File.ReadAllText(Path.Combine(folderPath, fileName));
        fileNameEntry.Text = fileName;
    }

    private void Delete_Clicked(object sender, EventArgs e)
    {
        string fileName = (string)((MenuItem)sender).BindingContext;
        File.Delete(Path.Combine(folderPath, fileName));
        UpdateFilesList();
    }
    private void ToList_Clicked(object sender, EventArgs e)
    {
        string fileName = (string)((MenuItem)sender).BindingContext;
        List<string> list = File.ReadLines(Path.Combine(folderPath, fileName)).ToList();
        listFailist.ItemsSource = list;
    }
    private async void Button_Main(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }
    private void DeleteButton_Clicked(object sender, EventArgs e)
    {
        if (FilesList.SelectedItem != null)
        {
            string fileName = FilesList.SelectedItem.ToString();
            string filePath = Path.Combine(folderPath, fileName);
            File.Delete(filePath);
            UpdateFilesList();
        }
        else
        {
            DisplayAlert("Error", "Kustutamiseks ei ole valitud ühtegi faili.", "OK");
        }
    }
}