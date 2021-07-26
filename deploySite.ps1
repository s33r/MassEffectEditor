$messageDate=Get-Date -Format "yyyy-MM-dd HHmm"

git config --global user.name 'Aaron Willows'
git config --global user.email 'aaron@aaronwillows.com'

rm -rf ./docs
cp -r ./Aaron.MassEffectEditor.WebUI/bin/Debug/net5.0/publish/wwwroot ./docs

git fetch
git switch site
git add docs
git commit -m "Release $messageDate"
git push
