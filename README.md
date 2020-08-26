# Zagreb Project

This project is a mediator that wakes-up when a PR created on a public GitHub repo (such as, polatengin/dotfiles, microsoft/vsts-extension-retrospectives, gruntwork-io/terratest, etc.) and trigger a GitHbub Action on a different repo (such as, CI workflow on microsoft/vsts-extension-retrospectives, or, Code Analysis workflow on microsoft/typescript, or, Tests workflow on gruntwork-io/terratest)

- Actors
  - Source Repo (for example, gruntwork-io/terratest)
  - Special Repo (which has the target GitHub Action)
  - Forked Repo (fork of the Source Repo)
  - PR (created on Forked Repo against Source Repo)
  - Pipeline (runs on Special Repo)
  - Azure Function (mediator between Source Repo and Special Repo)

- Flow
  - Source Repo forked on Forked Repo
  - New branch created on Forked Repo
  - Development happened on the new branch on Forked Repo
  - PR created on Forked Repo against Source Repo, so PR is available on Source Repo
  - Source Repo triggers the mediator Azure Function via WebHook
  - Azure Function triggers the Pipeline on the Special Repo
  - Pipeline run jobs & steps, collect the output
  - Pipeline make a comment on the PR on Source Repo
