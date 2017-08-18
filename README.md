# The Burrowers

**REQUIREMENTS**
* Git Installed https://git-scm.com/
* Git LFS installed https://git-lfs.github.com/ (Git may have been installed with this! Check if it has by running the command `git lfs version` in your terminal of choice)
* Latest Unity Version installed https://unity3d.com/
* GitHub Desktop installed (Not really necessary but this is what I'm explaining the steps with) https://desktop.github.com/
* GitHub Account created

**FIRST TIME SETUP**
1. Open GitHub Desktop and log in, then select the `Clone Repository` Option
2. Select the local path on your machine where you want the project (it will make its own folder)
3. Enter https://github.com/ScottPeckover/The_Burrowers into the URL section and press `Clone`
4. After cloning change the current branch to `origin/develop`
5. Navigate to the root project folder in your terminal of choice and run the command `git lfs install`
6. Create the following file and add to the root folder of the project:

*.gitconfig*

```
[merge]
tool = unityyamlmerge

[mergetool "unityyamlmerge"]
trustExitCode = false
cmd = 'C:\Program Files\Unity\Editor\Data\Tools\UnityYAMLMerge.exe' merge -p "$BASE" "$REMOTE" "$LOCAL" "$MERGED"
```
**__(IMPORTANT! `C:\Program Files\Unity\Editor\Data\Tools\UnityYAMLMerge.exe` may have a different location depending on your install of Unity. Modify this line for your system)__**

7. Open Unity, select `OPEN`, and select your project folder
8. ...
9. Profit!
