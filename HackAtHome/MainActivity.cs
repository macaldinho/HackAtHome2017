using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using HackAtHome.Entities;

namespace HackAtHome
{
    [Activity(Label = "@string/ApplicationName", MainLauncher = true, Icon = "@drawable/barba")]
    public class MainActivity : BaseActivity
    {
        EditText _txtEmail;
        EditText _txtPwd;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            var btnValidate = FindViewById<Button>(Resource.Id.buttonValdate);
            _txtEmail = FindViewById<EditText>(Resource.Id.editTextEmail);
            _txtPwd = FindViewById<EditText>(Resource.Id.editTextPassword);

            btnValidate.Click += BtnValidate_Click;
        }

        async void BtnValidate_Click(object sender, System.EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_txtEmail.Text) || string.IsNullOrWhiteSpace(_txtPwd.Text))
            {
                Toast.MakeText(this, Resources.GetString(Resource.String.Validation), ToastLength.Short).Show();
            }

            else
            {
                var deviceId = Android.Provider.Settings.Secure.GetString(ContentResolver, Android.Provider.Settings.Secure.AndroidId);

                var response = await Helper.Authenticate(_txtEmail.Text, _txtPwd.Text, deviceId);

                if (response.result.Status == Status.Success)
                {
                    var activity = new Intent(this, typeof(EvidenceActivity));

                    activity.PutExtra("Token", response.result.Token);
                    activity.PutExtra("User", response.result.FullName);

                    StartActivity(activity);
                }
                //Eto es para checar si hubo un error en la insercion de la evidencia en la parte de microsoft
                //O si ocurre un error con la parte de autenticacion de TICapacitacion, como no estan ligados los
                //2 metodos puede que la utentificacion sea correcta y la de microsoft falle entonces mostraremos el mensaje
                //pero si solo falla en ti capacitacion ya no entra al metodo de microsoft y manda el error en la parte de TI Capacitacion
                if (!string.IsNullOrWhiteSpace(response.message))
                {
                    Toast.MakeText(this, response.message, ToastLength.Short).Show();
                }
            }

        }
    }
}

