<%@ Page Language="C#" AutoEventWireup="true" CodeFile="manageData.aspx.cs" Inherits="cto.ctoadmin_manageData" %>

<!DOCTYPE html>
<!--[if lt IE 9]><html class="no-js lt-ie9" lang="en" dir="ltr"><![endif]--><!--[if gt IE 8]><!-->
<html class="no-js" lang="en" dir="ltr">
<!--<![endif]-->
<head>
    <meta charset="utf-8">
    <!-- Web Experience Toolkit (WET) / Boîte à outils de l'expérience Web (BOEW)
    wet-boew.github.io/wet-boew/License-en.html / wet-boew.github.io/wet-boew/Licence-fr.html -->
    <title>Cells, tissues and organs (CTO) inspections</title>
    <meta content="width=device-width,initial-scale=1" name="viewport">
    <!-- Meta data -->
    <meta name="dcterms.language" title="ISO639-2" content="eng" />
    <meta name="dcterms.title" content="Cells, tissues and organs (CTO) inspections" />
    <meta name="description" content="Find the latest results from the Government's cells, tissues and organs (CTO) inspections." />
    <meta name="dcterms.subject" title="scheme" content="" />
    <meta name="dcterms.creator" content="Government of Canada, Health Canada and the Public health Agency of Canada" />
    <meta name="dcterms.contributor" content="" />
    <meta name="dcterms.type" title="gctype" content="" />
    <meta name="dcterms.audience" title="gcaudience" content="" />
    <meta name="dcterms.issued" title="W3CDTF" content="2016-03-30" />
    <meta name="dcterms.modified" title="W3CDTF" content="2016-03-30" />
    <meta name="review_date" content="" />
    <meta name="meta_date" content="" />
    <!-- Load closure template scripts -->
    <script type="text/javascript" src="https://www.canada.ca/etc/designs/canada/cdts/gcweb/v4_0_22/cdts/compiled/soyutils.js"></script>
    <script type="text/javascript" src="https://www.canada.ca/etc/designs/canada/cdts/gcweb/v4_0_22/cdts/compiled/wet-en.js"></script>
    <script type="text/javascript" src="https://www.canada.ca/etc/designs/canada/cdts/gcweb/v4_0_22/cdts/compiled/plugins-en.js"></script>
    <!--<script type="text/javascript" src="./gcweb_4.0.22/wet-en.js"></script>
    <script type="text/javascript" src="./gcweb_4.0.22/plugins-en.js"></script>-->
    <!-- Load closure template scripts -->
    <noscript>
        <link rel="import" href="./staticFallbackFiles/gcweb/refTop.html">
    </noscript>
    <!-- Write closure template -->
    <script type="text/javascript">
            document.write(wet.builder.refTop({
                cdnEnv: "prod"
            }));
    </script>
    <link rel="stylesheet" href="./css/hcans.css" />
    <link rel="stylesheet" href="./css/inspections.css" />
    <!-- Google Tag Manager DO NOT REMOVE OR MODIFY - NE PAS SUPPRIMER OU MODIFIER -->
    <script>dataLayer1 = [];</script>
    <!-- End Google Tag Manager -->
    <script>
        function getParameterByName(name) {
            name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
            var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
                results = regex.exec(location.search);
            return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
        }
    </script>
</head>
<body vocab="http://schema.org/" typeof="WebPage" class="secondary">
    <!-- Google Tag Manager DO NOT REMOVE OR MODIFY - NE PAS SUPPRIMER OU MODIFIER -->
    <noscript>
        <iframe title="Google Tag Manager" src="//www.googletagmanager.com/ns.html?id=GTM-TLGQ9K" height="0" width="0" style="display:none;visibility:hidden"></iframe>
    </noscript>
    <script>
(function(w,d,s,l,i){w[l]=w[l]||[];w[l].push({'gtm.start': new Date().getTime(),event:'gtm.js'});var f=d.getElementsByTagName(s)[0], j=d.createElement(s),dl=l!='dataLayer1'?'&l='+l:'';j.async=true;j.src='//www.googletagmanager.com/gtm.js?id='+i+dl;f.parentNode.insertBefore(j,f);})(window,document,'script','dataLayer1','GTM-TLGQ9K');
    </script>
    <!-- End Google Tag Manager -->
    <div id="def-top">
        <link rel="import" href="./staticFallbackFiles/gcweb/top-en.html">
    </div>
    <script src="./js/inspections.js"></script>
    <script type="text/javascript">
			var defTop = document.getElementById("def-top");
			defTop.outerHTML = wet.builder.top({
				cdnEnv: "prod",
				lngLinks: [{
					lang: "fr",
					href: "manageData.aspx",
					text: "Français"
				}],
				breadcrumbs: [{
				    title: "Home",
				    href: "http://www.canada.ca/en/index.html"
				}, {
				    title: "Health",
				    href: "http://healthycanadians.gc.ca/index-eng.php"
				}, {
				    title: "Drug and health products",
				    href: "http://healthycanadians.gc.ca/drugs-products-medicaments-produits/index-eng.php"
				}, {
				    title: "Inspecting and monitoring drug and health products",
				    href: "http://healthycanadians.gc.ca/drugs-products-medicaments-produits/inspecting-monitoring-inspection-controle/index-eng.php"
				}, {
				    title: "Drug and health product inspections",
				    href: "http://healthycanadians.gc.ca/drugs-products-medicaments-produits/inspecting-monitoring-inspection-controle/inspections/index-eng.php"
				}]
			});
    </script>
    <div class="container">
    <main role="main" property="mainContentOfPage" class="container">
        <h1 property="name" id="wb-cont">Inspections des cellules, des tissus et des organes (CTO) for admin</h1>
         
       <div class="wb-frmvld">
        <form id="form1" runat="server">
            <section class="panel panel-default">
                    <div class="panel-heading">
                        <h2 class="panel-title">Maintain Data</h2>
                    </div>
                    <div class="panel-body">                   
                         <p>This is only for administrator in order to maintain data</p>
                         <div class="marginTop10Red marginbottom10">
                            <asp:Literal ID="outputTxt" runat="server"  ></asp:Literal>
                        </div> 
                         <div class="form-group">
                                <asp:Label ID="txtworkType" runat="server" CssClass="required col-sm-4 control-label" AssociatedControlId="workType">
                                    <span class="field-name">Work type</span>
                                    <strong class="required">(required)</strong>
                                </asp:Label>
                                <div class="col-sm-8 marginbottom10">
                                    <asp:DropDownList ID="workType" runat="server" CssClass="form-control" required="required">
                                        <asp:ListItem Text="Select a work type" Value=""></asp:ListItem>
                                      <asp:ListItem Text="Inspections" Value="1"></asp:ListItem>
                                        <%--  <asp:ListItem Text="Initial inspection deficiencies" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Report card summary" Value="3"></asp:ListItem>--%>
                                        <asp:ListItem Text="Inspections with Initial inspection deficiencies" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="Inspections with Report card summary" Value="5"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                        </div>   
                        <div class="clear"></div>              
                        <div class="form-group">
                            <label for="file1" class="required col-sm-4 control-label">
                                <span class="field-name">Excel file</span>
                                <strong class="required">(required)</strong>
                            </label>
                            <div class="col-sm-8 marginbottom10">
                                <input  id="file1" type="file" name="file1" runat="server" class="form-control" required="required" />
                            </div>
                      </div>
                      <div class="clear"></div>
                      <div class="form-group">
                            <label for="password1" class="required col-sm-4 control-label">
                                <span class="field-name">Password</span>
                                <strong class="required">(required)</strong>
                            </label>
                           <asp:CustomValidator ID="CustomValidator1"  onservervalidate="CustomValidator1_ServerValidate"   ControlToValidate="password1" runat="server"
                                ErrorMessage="Please enter a valid password" Display="Dynamic" CssClass="label-danger"></asp:CustomValidator> 
                            <div class="col-sm-8 marginbottom10">
                               <asp:TextBox  ID="password1" runat="server" CssClass="form-control" required="required" type="password"  maxlength="7"  size="7"  minlenght="7"  pattern=".{7,7}" data-rule-rangelength="[7,7]"></asp:TextBox> 
                            </div>
                      </div>
                      <div class="clear"></div>                    
                      <div class="form-group text-center">
                        <asp:Button ID="submit" runat="server"  Text="Submit"  class="btn btn-primary" OnClick="submit_Click"/>
                        <input type="reset" value="Reset" class="btn btn-default">
                     </div>
                </div>                 

                 <div class="panel-heading">
                        <h2 class="panel-title">Report on posting times</h2>
                 </div>
                <div class="panel-body">
                    <p>Below are the results for CTO inspections conducted since 2015-03-30</p>
                    <p class="col-sm-4"><a href="./reportResult-en.html?lang=en" class="btn btn-default">CTO inspections report</a></p>              
                    <div class="col-sm-8"><asp:Button ID="btnCreateExcel" runat="server"  Text="Download excel file"  class="btn btn-default" OnClick="btnCreateExcel_Click" CausesValidation="False" /></div>
                </div>
             </section>
        </form>
     </div>
       <div id="def-preFooter">
                <link rel="import" href="./staticFallbackFiles/gcweb/prefooter-en.html">
            </div>
            <!-- prefooter-en.html-End-->
            <!-- Write closure template -->
            <script type="text/javascript">
                var defPreFooter = document.getElementById("def-preFooter");
                defPreFooter.outerHTML = wet.builder.preFooter({
                    cdnEnv: "prod",
                    dateModified: "2016-11-08",
                    showPostContent: true,
                    showShare: true,
                });
            </script>
        </main>
    </div>

    <!-- footer-en.html-Start-->
    <div id="def-footer">
        <link rel="import" href="./staticFallbackFiles/gcweb/footer-en.html">
    </div>
    <!-- footer-en.html-End-->
    <!-- Write closure template -->
    <script type="text/javascript">
            var defFooter = document.getElementById("def-footer");
            defFooter.outerHTML = wet.builder.footer({
                cdnEnv: "prod",
                showFeatures: true,
            });
    </script>
    <!-- Write closure template -->
    <script type="text/javascript">
                document.write(wet.builder.refFooter({
                    cdnEnv: "prod"
                }));
    </script>
    <script src="../js//inspections.js"></script>
    <!--[if lt IE 10 | !IE ]><!-->
    <script src="../js//jquery.xdomainrequest.min.js"></script>
    <!-- GA Code Start -->
    <script src="/alt/js/ga-addon.js" type="text/javascript"></script>
    <script>
        var _gaq = _gaq || [];
        _gaq.push(['_setAccount', 'UA-21671527-4']);
        _gaq.push(['_gat._anonymizeIp']);
        _gaq.push(['_setDomainName', 'none']);
        _gaq.push(['_setAllowLinker', true]);
        _gaq.push(['_trackPageview']);
        _gaq.push(['_trackDownload']);	// Recently added
        _gaq.push(['_trackOutbound']);	// Recently added

        (function () {
            var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
            ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
            var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
        })();
    </script>
    <!-- GA Code Ends -->
    <script>
        $('#btnCreateExcel').click(function () {
            $('[required]').attr('required', false);
        });
    </script>
</body>
</html>