The default project would not compile as the package dependencies are not downloaded automatically.
You need to enable nuget package restore on Visual Studio solution. To enable it
1. Right Click the Solution
2. Select 'Enable Nuget Package Restore'.
3. Build the solution.

If you do not get this option please check you Pacakge Management setting.
1. Goto Tools-> Options.
2. Select 'Package Manager' from the list on the left.
3. Check the only item available under section 'Package Restore'.