using System.Text;
using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Webkit;
using Android.Widget;

namespace HackAtHome
{
    [Activity(Label = "@string/ApplicationName")]
    public class EvidenceDetailActivity : BaseActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.EvidenceDetail);

            SetUserName();
            LoadActivityDetail();
        }

        void SetUserName()
        {
            var user = FindViewById<TextView>(Resource.Id.textViewUserEvDe);
            user.Text = Intent.GetStringExtra("User");
        }

        async void LoadActivityDetail()
        {
            var evidenceId = Intent.GetIntExtra("EvidenceId", 0);
            var token = Intent.GetStringExtra("Token");
            var evidenceTitle = Intent.GetStringExtra("EvidenceTitle");
            var evidenceStatus = Intent.GetStringExtra("EvidenceStatus");
            var textViewTitle = FindViewById<TextView>(Resource.Id.textViewEvidenceTitle);
            textViewTitle.Text = evidenceTitle;
            var textViewStatus = FindViewById<TextView>(Resource.Id.textViewEvidenceStatus);
            textViewStatus.Text = evidenceStatus;

            var evidenceDetail = await Helper.GetEvidenceById(token, evidenceId);

            var webViewDescription = FindViewById<WebView>(Resource.Id.webViewEvidenceDescription);
            webViewDescription.SetBackgroundColor(Color.Transparent);

            var description = new StringBuilder();
            description.Append("<div style='color: white;'>")
                .Append(evidenceDetail.Description)
                .Append("</div>");

            webViewDescription.LoadDataWithBaseURL(null, description.ToString(), "text/html", "utf-8", null);

            var image = FindViewById<ImageView>(Resource.Id.imageViewEvidence);
            Koush.UrlImageViewHelper.SetUrlDrawable(image, evidenceDetail.Url);
        }
    }
}