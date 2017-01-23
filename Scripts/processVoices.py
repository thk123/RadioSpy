import string
from subprocess import call
import sys

voiceSettings = [" -vfemale1 -p 80 -s 200", #G(female)
		 " -vfemale3 -p 60 -s 200", #C(female)
		 " -vmale2 -p 120 -s 230", #V(male)
		 " -ven-uk-wi -p 40 -s 200", #F(male)
		 " -vfemale1 -p 100 -s 200", #R(female)
		 " -vfemale3 -p 40 -s 250", #A(female)
		 " -vfemale4 -p 60 -s 200", #L(female)
		 " -ven-uk-wmids -p 70 -s 230", #D(male)
		 " -vfemale5 -p 30 -s 230", #M(male)
		 " -ven-uk-sc -p 60 -s 230", #K(male)
		 ]
speakerMapping = dict(
					{
       				'G': 0,
        			'C': 1,
					'V': 2,
					'F': 3,
					'R': 4,
					'A': 5,
					'L': 6,
					'D': 7,
					'M': 8,
					'K': 9
    				})
espeakPath = r'"C:\Program Files (x86)\eSpeak\command_line\espeak.exe" --path="C:\Program Files (x86)\eSpeak" '

def readFile(fileName):
	fileNameStripped = fileName.split('.')
	
	lines = []
	currentConversationName = "ConversationA"
	f = open('../TwineSrc/'+fileName,'r')
	line = f.readline()
	while len(line):
		splitArray = str.split(line,'>')
		splitArrayLen = len(splitArray)

		if splitArrayLen != 0:
			
			for splitTag in splitArray:
				if (splitTag[0] == '<' and splitTag.find("tw-passagedata",1)!=-1):
					nameTagIndex = splitTag.find("name=")
					if(nameTagIndex != -1):
						nameTagIndex += 6 #skip name="
						nameTagEnd = splitTag.find('"',nameTagIndex);
						currentConversationName = splitTag[nameTagIndex:nameTagEnd];
						currentConversationName = currentConversationName.replace('.','_')
						currentConversationName = currentConversationName.replace(':','_')
			splitArray[-1]=splitArray[-1].replace( "&#39;","'")
			if (len(splitArray[-1])>1 and splitArray[-1][1] == ':'):
				speaker = speakerMapping.get( splitArray[-1][0] ,1)
				lines.append([speaker,splitArray[-1][3:-1],currentConversationName])
		line = f.readline()
	f.close()
	
	previousConversationName = ""
	blurbCount = 1
	for blurb in lines:
		if ( blurb[2] != previousConversationName):
			call ("mkdir ..\\Assets\\Audio\\Dialogue\\"+fileNameStripped[0]+'\\'+blurb[2], shell=True)
			previousConversationName = blurb[2]
			blurbCount=1
		#print(blurb)
		executeLine = espeakPath+" "+voiceSettings[blurb[0]]+" -w ../Assets/Audio/Dialogue/"+fileNameStripped[0]+'/'+blurb[2]+"/"+str(blurbCount).zfill(2)+".wav \""+blurb[1]+ "\""
		#print(executeLine)		
		call(executeLine, shell=True)
		blurbCount+= 1
		

if (len(sys.argv) > 1):
	readFile(sys.argv[1])
else:
	readFile("TheAffair.html")
	readFile("Revolutionaries.html")
	readFile("FloristsStory.html")

