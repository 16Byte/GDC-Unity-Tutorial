# GDCTut

Game Development Club tutorial project — a small 3D shooter sandbox built in
Unity 6 with URP, the new Input System, and a simple weapon pickup / enemy system.

---

## 1. What you need before you start

| Tool | Version | Notes |
|---|---|---|
| [Unity Hub](https://unity.com/download) | latest | Manages Unity installs |
| Unity Editor | **6000.3.23f1** | Exact version matters — see below |
| [Git](https://git-scm.com/downloads) | 2.40+ | That's it — no Git LFS needed |

The project is small and has no heavy art, so everything lives in plain git.
Clone it and you have the whole thing.

### Getting the right Unity version

Unity projects are **not** forward/backward compatible in a friendly way. If you
open this with a different editor version, Unity will silently upgrade the
project files and everyone else's next `git pull` will be a mess of churn.

In Unity Hub: **Installs → Install Editor → Archive → download archive**, and
pick `6000.3.23f1`. Add the **Windows Build Support (IL2CPP)** module if you
plan to make builds.

---

## 2. Getting the project

Unity Hub cannot clone a repository for you — it only opens folders that already
exist on disk. So it is two steps:

**Step 1 — clone it** (in a terminal, from wherever you keep your projects):

```bash
git clone https://github.com/16Byte/GDC-Unity-Tutorial.git GDCTut
```

**Step 2 — add it to Unity Hub:**

1. Open Unity Hub, go to the **Projects** tab
2. Click the **Add** dropdown, then **Add project from disk**
3. Select the `GDCTut` folder you just cloned (the one containing `Assets/`)
4. Check the **Editor Version** column reads `6000.3.23f1`, then click it to open

The first open takes several minutes — Unity is building the `Library/` folder
(asset import cache) from scratch. That folder is ~2 GB and is deliberately
**not** in git; every person generates their own.

Open `Assets/Scenes/SampleScene.unity` to get to the game.

---

## 3. Day-to-day workflow: branch, work, merge

We use a simple branch-and-pull-request flow. Nobody commits directly to `main`.

### Start a new piece of work

```bash
git checkout main
git pull
git checkout -b your-name/what-youre-doing
```

Branch naming: `diego/enemy-pathfinding`, `sam/shotgun-spread`. Your name up
front makes it obvious whose branch is whose in the GitHub list.

### While you work

Commit early and often — small commits are much easier to untangle than one
giant one.

```bash
git add .
git commit -m "Add spread pattern to shotgun fire"
```

**Close Unity (or at least save the scene, Ctrl+S) before you commit.** Unity
holds changes in memory and only writes them to disk on save, so an unsaved
scene means your commit is missing the work you just did.

### Push and open a pull request

```bash
git push -u origin your-name/what-youre-doing
```

Git prints a link to open a PR — click it, or go to the repo on GitHub and hit
**Compare & pull request**. Write a sentence about what changed and, for anything
visual, drag in a screenshot or clip.

The other person reviews, comments if needed, then clicks **Merge pull request**.

### After your PR is merged

```bash
git checkout main
git pull
git branch -d your-name/what-youre-doing
```

Then branch again for the next thing. Don't keep reusing an old branch.

### Keeping a long-running branch current

If `main` has moved on while you were working:

```bash
git checkout main
git pull
git checkout your-name/your-branch
git merge main
```

Fix any conflicts, commit, and push. Doing this regularly means small conflicts
instead of one huge one at the end.

---

## 4. Unity-specific rules that keep merges sane

These matter more in Unity than in most projects. Please actually read them.

### Always commit `.meta` files

Every asset has a matching `.meta` file holding its GUID — the ID that scenes and
prefabs use to reference it. Commit an asset without its `.meta` and everyone
else gets broken references. `git add .` handles this correctly; just don't
hand-pick files.

### Never commit `Library/`, `Temp/`, `Logs/`, or `UserSettings/`

Already handled by `.gitignore`. `Library/` alone is ~2 GB of machine-local
import cache.

### Coordinate on scenes and prefabs

`SampleScene.unity` is a single file. If two people edit it at the same time,
git cannot cleanly combine the changes — YAML merges on scenes are painful and
easy to corrupt.

Practical habits:

- **Say in chat when you're editing the shared scene**, and keep it short.
- **Prefer prefabs over scene edits.** Work inside `Assets/Prefabs/...` where
  you each own a different file, and the scene only needs to reference it.
- **Split work by file.** Two people on `Pistol.cs` and `Shotgun.cs` never
  conflict. Two people on `SampleScene.unity` always might.

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
whole and redo the other work by hand:

```bash
git checkout --theirs Assets/Scenes/SampleScene.unity
```

```bash
git checkout --ours Assets/Scenes/SampleScene.unity
```

Then `git add` the file to mark it resolved. (`--theirs` keeps the incoming
version, `--ours` keeps yours.)

### Don't change the Unity version without telling everyone

Upgrading the editor rewrites project files across the board. That's a
conversation and its own PR, not a drive-by change.

---

## 5. Project layout

```
Assets/
  Materials/      Shared materials (Player, Enemy, Wood, Metal, Hole)
  Plugins/        Third-party: PMG Proto-Grid prototyping textures
  Prefabs/
    Enemies/      Enemy.prefab
    Weapons/      Pistol, Shotgun, and their pickup variants
  Scenes/         SampleScene.unity — the playable scene
  Scripts/
    Enemies/      Enemy.cs, EnemyManager.cs
    Player/       PlaceholderPlayerController.cs, WeaponHolder.cs
    Weapons/      Weapon.cs (base), Pistol.cs, Shotgun.cs, WeaponPickup.cs
  Settings/       URP render pipeline assets, Input System actions
  Textures/       Crosshair.png
Packages/         UPM dependency manifest + lockfile (committed on purpose)
ProjectSettings/  Project-wide Unity settings (committed on purpose)
```

---

## 6. Troubleshooting

**Textures are pink / assets look broken.**
URP shaders didn't resolve. In Unity:
**Edit → Rendering → Materials → Convert All Built-in Materials to URP**.

**"The project was created with a different version of Unity."**
Don't click through it. Install `6000.3.23f1` from Unity Hub's archive and open
with that instead.

**Unity is behaving strangely after a pull / scripts won't compile.**
Close Unity, delete the `Library/` folder, and reopen. It rebuilds from scratch
(slow, but fixes most import weirdness) and is safe — nothing in `Library/` is
your work.

**`git pull` says my changes would be overwritten.**
Commit or stash them first:

```bash
git stash
git pull
git stash pop
```

**I committed something huge by accident.**
Stop and ask before pushing — it's much easier to fix before it reaches GitHub.
