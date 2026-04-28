# CSC 4444G Project

A Unity ML-Agents combat training project using Proximal Policy Optimization (PPO).

---

## Prerequisites

Install all required tools for your platform before opening the project.

### Unity (Required — both platforms)

| Tool         | Version         | Download                                   |
| ------------ | --------------- | ------------------------------------------ |
| Unity Hub    | Latest          | https://unity.com/download                 |
| Unity Editor | **6000.0.69f1** | Install via Unity Hub after installing Hub |

> **Important:** You must install exactly Unity **6000.0.69f1**. Other versions may cause compatibility issues.

### Python / ML Training (Optional — both platforms)

Only needed if you want to run or modify agent training. Not required to open or play the project.

| Tool      | Version                     | Download                                  |
| --------- | --------------------------- | ----------------------------------------- |
| Miniconda | Latest                      | https://www.anaconda.com/download/success |
| Python    | 3.10.12 (managed via conda) | —                                         |
| mlagents  | Latest                      | via pip                                   |
| PyTorch   | 2.8.0                       | via pip                                   |

**Mac note:** On the Miniconda download page, choose the installer for your chip — **Apple Silicon (M1/M2/M3)** or **Intel**, not both.

---

## Project Setup

### 1. Open the Project in Unity

1. Extract the submitted zip file to a location of your choice
2. Open **Unity Hub**
3. In the **Projects** tab, click **Add** (between "Search" and "New Project")
4. Select **Add project from disk**
5. Navigate to the extracted `4444-Project` folder and click **Open**
6. Unity Hub will open the project using the correct editor version

> **Blank scene?** If the project opens to an empty screen, go to `Assets/Scenes/` in the Project panel and double-click **SampleScene**.

---

## ML Training Setup

### 1. Create the Conda Environment

**Windows** — open **Anaconda Prompt** (search for it in the Start menu):

```bat
conda create -n mlagents python=3.10.12
conda activate mlagents
```

**Mac** — open **Terminal**:

```bash
conda create -n mlagents python=3.10.12
conda activate mlagents
```

### 2. Install Dependencies

Run the following inside the active `mlagents` environment (same on both platforms):

```bash
pip install mlagents
pip install torch==2.8.0
```

### 3. Start a Training Run

Navigate to the extracted project folder, then launch the ML-Agents trainer.

**Windows:**

```bat
cd C:\path\to\4444-Project
mlagents-learn config/ppo/MoveToGoal.yaml --run-id=<your-run-name>
```

**Mac:**

```bash
cd /path/to/4444-Project
mlagents-learn config/ppo/MoveToGoal.yaml --run-id=<your-run-name>
```

Replace `<your-run-name>` with any label you like (e.g. `run1`, `test_v2`).

Once the training server prints **"Start training by pressing the Play button in the Unity Editor"**, switch to Unity and press **Play**.

> **Run ID conflict?** Add `--force` to overwrite an existing run with the same name, or pick a different `--run-id`.

---

## Troubleshooting

| Issue                                        | Fix                                                                                |
| -------------------------------------------- | ---------------------------------------------------------------------------------- |
| Unity can't find the correct editor version  | Open Unity Hub → Installs → add version**6000.0.69f1**                             |
| `mlagents-learn` not found                   | Make sure the conda environment is active:`conda activate mlagents`                |
| Project opens to a blank screen              | Manually open `Assets/Scenes/SampleScene` from the Project panel                   |
| Mac:`conda` command not found after install  | Restart Terminal, or run `source ~/.zshrc` (zsh) / `source ~/.bash_profile` (bash) |
| Mac: PyTorch install errors on Apple Silicon | Try `pip install torch==2.8.0 --index-url https://download.pytorch.org/whl/cpu`    |
