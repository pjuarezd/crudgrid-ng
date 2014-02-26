using System.Web.Optimization;

namespace CrudGridExample.App_Start
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = false;

            bundles.Add(new ScriptBundle("~/bundles/scripts/vendor/tagManager")
                            .Include("~/Scripts/Vendor/bootstrap-tagsInput/bootstrap-tagsinput.js")
                            .Include("~/Scripts/Vendor/bootstrap-tagsInput/bootstrap-tagsinput-angular.js")
                );

            bundles.Add(new ScriptBundle("~/bundles/scripts/vendor/linqjs")
                            .Include("~/Scripts/Vendor/linq.js/linq.js")
                );

            bundles.Add(new ScriptBundle("~/bundles/scripts/vendor/chosen")
                            .Include("~/Scripts/Vendor/chosen/chosen.js")
                            .Include("~/Scripts/Vendor/chosen/chosen.jquery.js")
                );

            bundles.Add(new ScriptBundle("~/bundles/scripts/vendor/highcharts")
                            .Include("~/Scripts/Vendor/highcharts/highcharts-all.js")
                            .Include("~/Scripts/Vendor/highcharts/highcharts-ng.js")
                );

            bundles.Add(new ScriptBundle("~/bundles/scripts/vendor/jquery")
                            .Include("~/Scripts/Vendor/jQuery/jquery-{version}.js")
                            .Include("~/Scripts/Vendor/jQueryDateFormat/jquery-2.0.3.dateFormat.js")
                );

            bundles.Add(new ScriptBundle("~/bundles/scripts/vendor/bootstrap")
                            .Include("~/Scripts/Vendor/bootstrap/bootstrap.js")
                );

            bundles.Add(new ScriptBundle("~/bundles/scripts/vendor/bootstrapUI")
                            .Include("~/Scripts/Vendor/bootstrap-ui/*.js")
                );

            bundles.Add(new ScriptBundle("~/bundles/scripts/vendor/bootstrap-tagsInput")
                            .Include("~/Scripts/Vendor/bootstrap-tagsInput/*.js")
                );

            bundles.Add(new ScriptBundle("~/bundles/scripts/vendor/toastr").Include(
                "~/Scripts/Vendor/toastr/toastr.js"));

            bundles.Add(new ScriptBundle("~/bundles/scripts/vendor/angular")
                            .Include("~/Scripts/Vendor/angularJs/angular.js")
                            .Include("~/Scripts/Vendor/angularJs/angular-cookies.js")
                            .Include("~/Scripts/Vendor/angularJs/angular-loader.js")
                            .Include("~/Scripts/Vendor/angularJs/angular-resource.js")
                            .Include("~/Scripts/Vendor/angularJs/angular-route.js")
                            .Include("~/Scripts/Vendor/angularJs/angular-sanitize.js")
                            .Include("~/Scripts/Vendor/angularJs/angular-scenario.js")
                );
            
            bundles.Add(new StyleBundle("~/bundles/css/vendor/tagManager")
                .Include("~/Content/bootstrap-tagsinput.css"));

            bundles.Add(new StyleBundle("~/bundles/css/vendor/toastr")
                .Include("~/Content/toastr.css"));

            bundles.Add(new StyleBundle("~/bundles/css/vendor/bootstrap")
                            .Include("~/Content/bootstrap.css")
                );

            bundles.Add(
                new StyleBundle("~/bundles/css/vendor/angular")
                    .Include("~/Content/angular-csp.css")
                );

            bundles.Add(
                new StyleBundle("~/bundles/css/vendor/chosen")
                    .Include("~/Content/chosen-spinner.css")
                    .Include("~/Content/chosen.css")
                );

            bundles.Add(new ScriptBundle("~/bundles/scripts/crudgrid-ng")
                            .Include("~/Scripts/App/app.js",
                                     "~/Scripts/App/Directives/*.js",
                                     "~/Scripts/App/Directives/Services/*.js",
                                     "~/Scripts/App/Directives/Filters/*.js",
                                     "~/Scripts/App/Services/*.js",
                                     "~/Scripts/App/Controllers/*.js"
                            ));

            bundles.Add(new StyleBundle("~/bundles/css/crudgrid-ng")
                            .Include("~/Scripts/App/Directives/Content/*.css"));
        }
    }
}