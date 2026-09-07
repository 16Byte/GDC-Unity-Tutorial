# Game Development Club - Unity Tutorial - First Person Shooter

<img width="1280" height="720" alt="Image Sequence_001_0277" src="https://github.com/user-attachments/assets/52067c6f-4ace-4e75-94ee-d03b167e7393" />


Game Development Club tutorial project — a small 3D shooter sandbox built in
Unity 6 with URP, the new Input System, and a simple weapon pickup / enemy system.

---

## 1. What you need before you start

| Tool | Version | Notes |
|---|---|---|
| [Unity Hub](https://unity.com/download) | latest | Manages Unity installs |
| Unity Editor | **6000.3.23f1** | Exact version matters — see below |
| [Git](https://git-scm.com/downloads) | 2.40+ | That's it — no Git LFS needed |

---

## 2. Setting up the project

**Step 1 — clone it or download the latest [release](https://github.com/16Byte/GDC-Unity-Tutorial/releases/)**

```bash
git clone https://github.com/16Byte/GDC-Unity-Tutorial.git GDCTut
```

**Step 2 — add it to Unity Hub:**

1. Open Unity Hub, go to the **Projects** tab
2. Click the **Add** dropdown, then **Add project from disk**
3. Select the `GDCTut` folder you just cloned or downloaded from [releases](https://github.com/16Byte/GDC-Unity-Tutorial/releases/)
4. The project will now be added to Unity Hub, click on it to open it.
5. Once you're in the editor: Open `Assets/Scenes/SampleScene.unity` to get to the game.

You will be prompted to download the Unity Editor version tied to the project if you don't have it installed already.

Current Unity version is: **6000.3.23f1**

We'll try to keep this project at the latest Unity 6.3 LTS, if you notice it's out of date with the current LTS version, please let us know by writing an [issue](https://github.com/16Byte/GDC-Unity-Tutorial/issues/).



---

## 3. Contributing To This Repo

We use a simple branch-and-pull-request flow. Nobody commits directly to `main`.

### Start a new piece of work

```bash
git checkout main
git pull
git checkout -b (feature, fix, etc.)/what-youre-doing
```

Branch naming: `feature/enemy-pathfinding`, `fix/shotgun-spread`.

### Push and open a pull request

```bash
git push -u origin your-name/what-youre-doing
```

Git prints a link to open a PR — click it, or go to the repo on GitHub and hit
**Compare & pull request**. 

The other person reviews, comments if needed, then clicks **Merge pull request**.

### After your PR is merged

```bash
git checkout main
git pull
git branch -d (feature, fix, etc.)/what-youre-doing
```
The git branch -d branch-name command deletes the branch. If you need to work on the same feature in the future, you can recreate the branch with the exact same name.

---

## 4. Unity-specific rules that keep merges sane

These matter more in Unity than in most projects. Please actually read them.

### Always commit `.meta` files

### Never commit `Library/`, `Temp/`, `Logs/`, or `UserSettings/`

### Resolving scene/prefab conflicts with Unity's smart merge

Unity ships a merge tool that understands its YAML format. `.gitattributes` is
already configured to call it; set up the tool itself once per machine:

```bash
git config merge.unityyamlmerge.name "Unity SmartMerge"
```

```bash
git config merge.unityyamlmerge.driver "'C:/Program Files/Unity/Hub/Editor/6000.3.23f1/Editor/Data/Tools/UnityYAMLMerge.exe' merge -p %O %B %A %A"
```

Adjust the path if your Unity install lives elsewhere. On macOS the tool is at
`/Applications/Unity/Hub/Editor/6000.3.23f1/Unity.app/Contents/Tools/UnityYAMLMerge`.

If a scene conflict is still a mess, the safe escape hatch is to take one side
whole and redo the other work by hand.

### Don't change the Unity version without telling everyone

Upgrading the editor rewrites project files across the board. That's a
conversation and its own PR, not a drive-by change.
