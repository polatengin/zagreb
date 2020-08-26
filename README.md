# Zagreb Project

This project is a mediator that wakes-up when a PR created on a public GitHub repo (such as, polatengin/dotfiles, microsoft/vsts-extension-retrospectives, gruntwork-io/terratest, etc.) and trigger a GitHbub Action on a different repo (such as, CI workflow on microsoft/vsts-extension-retrospectives, or, Code Analysis workflow on microsoft/typescript, or, Tests workflow on gruntwork-io/terratest)

- Actors
  - Source Repo (for example, gruntwork-io/terratest)
  - Special Repo (which has the target GitHub Action)
  - Forked Repo (fork of the Source Repo)
  - PR (created on Forked Repo against Source Repo)
  - Pipeline (runs on Special Repo)
  - Azure Function (mediator between Source Repo and Special Repo)
