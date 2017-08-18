# The Burrowers

**FIRST TIME SETUP**

Create the following file and add to the root folder of the project:

*.gitconfig*
```
[merge]
tool = unityyamlmerge

[mergetool "unityyamlmerge"]
trustExitCode = false
cmd = 'C:\Program Files\Unity\Editor\Data\Tools\UnityYAMLMerge.exe' merge -p "$BASE" "$REMOTE" "$LOCAL" "$MERGED"
```

(Side note: in order to create this file on Windows just rename a new text file '.gitconfig.', removing the .txt extension)
