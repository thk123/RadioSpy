import string
from subprocess import call

voiceSettings = [" -vfemale1 -p 80 -s 200", #G(female)
		 " -vfemale3 -p 60 -s 200", #C(female)
		 " -vmale2 -p 120 -s 230", #V(male)
		 " -p 30 -s 200"] #F(male)
speakerMapping = dict({
       					'G': 0,
        				'C': 1,
					'V': 2,
					'F': 3
    					})
espeakPath = r'"C:\Program Files (x86)\eSpeak\command_line\espeak.exe" --path="C:\Program Files (x86)\eSpeak" '

def readFile(conversationName):
	
	call ("mkdir ../Assets/Audio/Dialogue/"+conversationName, shell=True)
	lines = []
	f = open('../TwineSrc/Revolutionaries.html','r')
	line = f.readline()
	while len(line):
		splitArray = str.split(line,'>')
		splitArrayLen = len(splitArray)

		if splitArrayLen != 0:
			splitArray[-1]=splitArray[-1].replace( "&#39;","'")
			if (len(splitArray[-1])>1 and splitArray[-1][1] == ':'):
				speaker = speakerMapping.get( splitArray[-1][0] ,1)
				lines.append([speaker,splitArray[-1][3:-1]])
		line = f.readline()
	f.close()
	
	blurbCount = 1
	for blurb in lines:
		print(blurb)
		executeLine = espeakPath+" "+voiceSettings[blurb[0]]+" -w ../Assets/Audio/Dialogue/"+conversationName+"/"+str(blurbCount).zfill(2)+".wav \""+blurb[1]+ "\""
		print(executeLine)		
		call(executeLine, shell=True)
		blurbCount+= 1
		
	

readFile("ConversationA")

