# 4444 Project

Windows Installation:
Download Unity Hub
Install Unity 6000.0.69f1

Install Github Desktop
Go to top of Github Desktop and press File -> Clone Repository
Go to URL and put this link: “https://github.com/RaviStimphil/4444-Project/” and make your local path. 
Clone the Repo.

Open Unity Hub, and in the project tab, press “Add,” which is between “Search” and “New Project.”
Add from Disk, and then go to the path that the repo was cloned. Make sure to select the folder, which should be named “4444-Project” and then open.
If project opens and its blank, go to the Scenes folder and click “SampleScene”

If you want to see how training go:
Download miniconda
In the anaconda prompt (Search “anaconda prompt” in computer search) “conda create -n mlagents python=3.10.12”
“conda activate mlagents” in the prompt

While in mlagents environment, put “pip install mlagents” and “pip install torch==2.8.0”.
Then go to the path of the project “cd C:\Users\Lyoko\Documents\GitHub\4444-Project” or “cd C:\Users\...\444-Project” Whatever the path is to the project.
“Mlagents-learn config/ppo/MoveToGoal.yaml --run-id=[Put whatever name you want]” to make a training run. Use --force to overwrite one with the same name or just use a different name. 

