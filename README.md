# Cocoon
An implementation of the Strangler Fig pattern for ASP.NET Core

## Using cocoon with fullframework MVC.

### Install the legacy nuget package
![Install legacy package](docs/images/install-legacy-package.png)

### Amend the web.config to load the handlers

```
<system.webServer>
    <handlers>
        <add name="FacadeSession" verb="*" path="facadesession" type="ReCode.Cocoon.Legacy.Session.SessionApiHandler, ReCode.Cocoon.Legacy, Version=1.0.0.0, Culture=neutral"  preCondition="integratedMode"/>
        <add name="FacadeCookies" verb="*" path="facadecookies" type="ReCode.Cocoon.Legacy.Cookies.CookieApiHandler, ReCode.Cocoon.Legacy, Version=1.0.0.0, Culture=neutral" preCondition="integratedMode"/>
        <add name="FacadeAuth" verb="*" path="facadeauth" type="ReCode.Cocoon.Legacy.Auth.AuthApiHandler, ReCode.Cocoon.Legacy, Version=1.0.0.0, Culture=neutral" preCondition="integratedMode"/>
    </handlers>
</system.webServer>
```

### Disable MVC routing for the new handlers

```
public static void RegisterRoutes(RouteCollection routes)
{
    routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
    routes.IgnoreRoute("favicon.ico");
    routes.IgnoreRoute("{*robotstxt}", new { robotstxt = @"(.*/)?robots.txt(/.*)?" });
    
    // Ignored routes for Cocoon 
    routes.IgnoreRoute("facadesession");
    routes.IgnoreRoute("facadeauth");
    routes.IgnoreRoute("facadecookies");
    // End ignored reoutes for Cocoon

    routes.MapMvcAttributeRoutes();
    AreaRegistration.RegisterAllAreas();

    routes.MapRoute(
        "Default", 
        "{controller}/{action}/{id}", 
        new { controller = "Home", action = "Index", id = UrlParameter.Optional },
        new[] { "TestApp.Controllers" });
}
```

### Check the session and auth cookie names are aligned between the applications

For authentication and session to work the names Cocoon uses between the applications must be aligned. In the new application look at the appSettings files.

```
"Cocoon": {
    "Proxy": {
      "DestinationPrefix": "https://localhost:44302/"
    },
    "Authentication": {
      "BackendApiUrl": "https://localhost:44302/facadeauth",
      "LoginUrl": "/Account/Login?ReturnUrl={{ReturnUrl}}",
      "Cookies": [
        "AuthCookie"
      ]
    },
    "Session": {
      "BackendApiUrl": "https://localhost:44302/facadesession",
      "Cookies": [
        "ASP.NET_SessionId"
      ]
    },
    "Cookies": {
      "BackendApiUrl": "https://localhost:44302/facadecookies"
    }
  }
```

The cookie names need to match the names in the application that's being facaded. They can normally be found in the web.config

```
 <authentication mode="Forms">
    <forms loginUrl="~/Account/LogOn" timeout="432000" name="COOKIENAME" slidingExpiration="true" />
</authentication>
```

```
<system.web>
    <sessionState regenerateExpiredSessionId="false" cookieless="UseCookies" cookieName="COOKIENAME" />
</system.web>
```