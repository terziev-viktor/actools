using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Markup;

[assembly: AssemblyTitle("AcManager.InternalWrapper")]
[assembly: AssemblyDescription("Wrapper over AcManager.Internal for Content Manager")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("AcClub")]
[assembly: AssemblyProduct("AcManager.InternalWrapper")]
[assembly: AssemblyCopyright("Copyright © AcClub, 2015-2026")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
[assembly: Guid("12dac3d3-b40c-4532-9ab4-7ff939fdd993")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

// Same XAML namespace as the original, used as xmlns:in="http://acstuff.club/app/internal"
[assembly: XmlnsDefinition("http://acstuff.club/app/internal", "AcManager.Internal")]
[assembly: XmlnsPrefix("http://acstuff.club/app/internal", "in")]
