using Android.OS;
using Android.Support.V4.App;
using Android.Views;

namespace Xamarin.Examples.Demo.Droid.Fragments.Base
{
    public abstract class ExampleBaseFragment : Fragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(ExampleLayoutId, null);
        }

        public abstract int ExampleLayoutId { get; }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            InitExample();
        }

        protected abstract void InitExample();

        public virtual void InitExampleForUiTest()
        {
        }
    }
}