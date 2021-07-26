$messageDate = Get-Date -Format "yyyy-MM-dd HHmm"

cp -r ./Aaron.MassEffectEditor.WebUI/bin/Debug/net5.0/publish/wwwroot ./docs
git switch site
git add docs
git commit -m "Release $messageDate"
git push
