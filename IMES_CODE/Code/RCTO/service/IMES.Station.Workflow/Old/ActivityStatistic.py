#coding: gbk
import os
resultfile = open('result.txt', 'w')
special = open('special.txt', 'w')
common = open('common.txt', 'w')
actf = open('activities.txt', 'r')
for activityname in actf:
    activityname = activityname.rstrip('\n')
    resultfile.write(activityname)
    resultfile.write('    ')
    count = 0
    print(activityname)
    wff = open('wfs.txt', 'r')
    for wfname in wff:
        find = 0
        wfname = wfname.rstrip('\n')
        if len(wfname) > 0:
            wffile = open(wfname, 'r')
            for wfline in wffile:
                linefind = wfline.rfind(activityname)
#                print(activityname)
#                print(wfline)
#                print(linefind)
                if linefind != -1:
                    find = 1
#                    resultfile.write(wfname)
#                    resultfile.write('  ')
            wffile.close()
            if find == 1:
                count = count + 1
    resultfile.write('    ')
    resultfile.write('{0}'.format(count))
    resultfile.write('\n')
    if count > 1:
        common.write(activityname)
        common.write('\n')
    else:
        special.write(activityname)
        special.write('\n')
    wff.close()
actf.close()
resultfile.close()
special.close()
common.close()
                           
