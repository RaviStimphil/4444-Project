# 4444 Project

A Unity ML-Agents combat training project using Proximal Policy Optimization (PPO).

There are two ways to run this project — pick the one that fits your goal:

- **[Option A](#option-a-run-in-unity-no-training)** — Just open and play the project in Unity. No Python required.
- **[Option B](#option-b-run-with-ml-agent-training)** — Run live agent training alongside Unity using ML-Agents.

---

## Option A: Run in Unity (No Training)

Use this to open the project, inspect the scene, and watch pre-trained agent behavior.

### Prerequisites

| Tool         | Version         | Download                                   |
| ------------ | --------------- | ------------------------------------------ |
| Unity Hub    | Latest          | https://unity.com/download                 |
| Unity Editor | **6000.0.69f1** | Install via Unity Hub after installing Hub |

> **Important:** Install exactly Unity **6000.0.69f1**. Other versions may cause compatibility issues.

### Setup

1. Extract the submitted zip file to a location of your choice
2. Open **Unity Hub**
3. In the **Projects** tab, click **Add** (between "Search" and "New Project")
4. Select **Add project from disk**
5. Navigate to the extracted `4444-Project` folder and click **Open**
6. Once the project loads, press **Play** in the Unity Editor to run it

> **Blank scene?** If the project opens to an empty screen, go to `Assets/Scenes/` in the Project panel and double-click **SampleScene**, then press **Play**.

---

## Option B: Run with ML Agent Training

Use this if you want to train the agents from scratch or resume a training run.
This requires Python in addition to Unity.

### Prerequisites

**Unity (same as Option A):**

| Tool         | Version         | Download                                   |
| ------------ | --------------- | ------------------------------------------ |
| Unity Hub    | Latest          | https://unity.com/download                 |
| Unity Editor | **6000.0.69f1** | Install via Unity Hub after installing Hub |

**Python / ML-Agents:**

| Tool      | Version                     | Download                                  |
| --------- | --------------------------- | ----------------------------------------- |
| Miniconda | Latest                      | https://www.anaconda.com/download/success |
| Python    | 3.10.12 (managed via conda) | —                                         |
| mlagents  | Latest                      | via pip                                   |
| PyTorch   | 2.8.0                       | via pip                                   |

**Mac note:** On the Miniconda download page, choose the installer for your chip — **Apple Silicon (M1/M2/M3)** or **Intel**, not both.

### Setup

#### 1. Open the Project in Unity

Follow the same steps as Option A (steps 1–5). Do **not** press Play yet.

#### 2. Create the Conda Environment

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

#### 3. Install Dependencies

Run the following inside the active `mlagents` environment (same on both platforms):

```bash
pip install mlagents
pip install torch==2.8.0
```

#### 4. Start a Training Run

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

Once the trainer prints **"Start training by pressing the Play button in the Unity Editor"**, switch to Unity and press **Play**.

> **Run ID conflict?** Add `--force` to overwrite an existing run with the same name, or choose a different `--run-id`.

---

## Troubleshooting

| Issue                                        | Fix                                                                                 |
| -------------------------------------------- | ----------------------------------------------------------------------------------- |
| Unity can't find the correct editor version  | Open Unity Hub → Installs → add version **6000.0.69f1**                             |
| Project opens to a blank screen              | Manually open `Assets/Scenes/SampleScene` from the Project panel                    |
| `mlagents-learn` not found                   | Make sure the conda environment is active: `conda activate mlagents`                |
| Mac: `conda` command not found after install | Restart Terminal, or run `source ~/.zshrc` (zsh) / `source ~/.bash_profile` (bash)  |
| Mac: PyTorch install errors on Apple Silicon | Try `pip install torch==2.8.0 --index-url https://download.pytorch.org/whl/cpu`     |
