using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace SVM
{                                                                                                                                                      
    #region Today is speedy day
 //                             ..-::::-.`                                                                                                              
 //                        `-/osssssssssssso/-                                                                                                          
 //                      -+ssssssssssssssssssss/`                                                                                                       
 //                    -sssssssossssssssossssssso.                                                                                                      
 //                   .ssssss+s+ossssssoosososssss:                                                                                                     
 //                   +ssssoyos+ssssssoo+oohossssss-                                                                                                    
 //                   sssssodyso+oossos+ssdys+o+ooyh.                                                                                                   
 //                   ss+o+s+m+soyoss+hoosdooh+soosNy                                                                                                   
 //                  .yosohoosmsoosssssoohmoossyyossm/```                                                                                               
 //                  :syhyssomsosssssssss+mmooodoososmdhyso:.                                                                                           
 //                 /oosNsssdNhsoosssooosdmNmmmNNdydmddhhyyys:                                                                                          
 //               -smdhdNNNNNNNmdhyyyyyyyyhhhyyyyyyyyoshddhyhyo-sdddddddh.                                                                              
 //             .sdmddosossyyyyyysosso+//+osyyyyyyyssooodhyyyys++:.oMd``` .:++/-  `+.-/+:`  .:++/-  .+.:++-       .:++/.  .+.:++-                       
 //            -hddddds:/osssyyyyyss+//:+sssyyyyyyysso/ohhhyyyyo++/oMd   .my//yNo -Mmo/yMy .my//yNo oMd+/dN+     +Nh//yN+ oMd+/dN+                      
 //           -yhddddds//+osyyyyyssso+:-:osssyyyyyyso+/ydhyhhhyso+:+Md   `oooshMy :Md  .Mm `oooshMy oMo  +Ms     NM.  `MM oMo  +Ms                      
 //          :hhhhhddhs+++osyyyyysso++/::/osssyyyyys+/+smdhyyhyso:`+Md   oMy:/yMd :Mh  .Mm oMy:/yMd oMo  +Ms     oMs--sMs oMo  +Ms                      
 //         -yhhhdddhyo++//+osssooooo+////+sssssso++/+ooyhhhyyys:  .s/   `+so/.+o `s/   s+ `+so/.+o -s-  .s:      -+sso-  -s-  .s:                      
 //        :yhhdddddysso+////++++++ooo++++osso+ooooooooosyyyyyyo                                                                                        
 //       :yhhhhhdddyssoo+++++ossoossooooosssssshhyysssoosyyyy+.                                                                                        
 //       .+yhyhhhyyysssooosydddyyyyssssssssyhdmmmmmdhhsooyo/-`                                                                                         
 //         -oyyhhhysssyhddmmmmmNmmmmmddddmmmNmmmdmmmddy+so`                               yy       /h.  -h:                       -h:                  
 //           //+hysssydmmmmmmmmNNNmmmNNNmNNmmdmmmmmdhy++ss-    ./`   /- `-///:`  ::   :/  MM.:/:` -hMo- .s-     .- -:-`   .://:.  .s- /-   ./` .://:.  
 //              /ssssshmmNNNNmmmmmhhdmddddddhddNNmhsoooooo:    :My  +M+ hd+/sNy  MN   mM` MMs/oNm`:dMy/ +Mo     hNyoohm+ :ms++mN- +Mo dN. `mm`:ms++mN- 
 //              .oossoosyhmmNNNNNNmdddhhddmmmNNdyoooo++++o.     oM/-Ny  +o+osMN  MM   NM` MM   mM` yM/  oMo     dM/  `MM -o+oodM+ oMo .md`yN- -o+oodM+ 
 //               o++oooooooossssssyyhhdddmmmmhsoosso++/++/       hNmd` :Nd/:oMN` NM/-/MM` MM   mM` sMo-`oMo     dMy--oMh hMo:/dMo oMo  :NdM/  hMo:/dMo 
 //               /o+++++++oosooo++///:///++o+++syys++++++.       `ss.  `+ys+-/y` -syo/+s  ss   os  .sys--y-     dM/oss/` -oyo/-s+ -y-   :y/   -oyo/-s+ 
 //               -so+//++++ossyssooo+++////+osyyso++++++-                                                       hN`                                    
 //               .o+oo++++++++osyyyyyssssyyyyysoooo+oo+.                                                        ..                                     
 //           `./sd/-+oooooo++++++oosssssssssooooooooo/```                                                                                              
 //        ./sdmNNm.../ooooooooo+++++oooooooooooooooo-` -ho:`                                                                                           
 //        oyyNNNNd```.:+oosssooooooooooooooooooooo+-````+NN+                                                                                           
 //       `-:/yNNNy````.-/++ooossoooooooossoooooo+/syhyo:`yN-                                                                                           
 //      ```.-/sNNo.````.:+ossssooooooooooooooo+++sNNNNNms/d. `                                                                                         
 //      ```.--.oNo...:ohmNms+++::/+:-/++/:---.--.smmNNNNNds-`                                                                                          
 //     `````..`.ys-+hNNNNNNm+.-.`````...````.````yNNNNNNNNs:`  `                                                                                       
 //    ```````.``-hmNNNNNNNNNNs-..`````.`````.`` ``omNNNNNNm:`` `                                                                                       
 //    ```.````.``/NNNNNNNNNNNNd/.``..```````..`` ``:hNNNmNNo`` ``                                                                                      
 //   ````.````.``.hNNMNMMNMNNNNd```..```````.-``  ``.sNNNNNm-`  `                                                                                      
 //   ``....```````+NNNNNNMMNNNN/```.--.``````-.``` ```sNNNNNh`````                                                                                     
 //   `...-..```.``-yNNNNNNNNNmh...----..``` `.-```` ```sNNNNNo.-.``        ``                                                                          
 //  ``.--:--.``````/mNNmmmmmdddhdmmdso++//os+/+:--...---mmmNNN+:-.``  ```    `                                                     ``                  
 //  `...-/+:-.```..:hNmddmdmmdhhhhdhdmddddmdhdmmhdddddddNmNNNNm+/:.` ````      ``                                                `-.-.                 
 //  ``.``./++:.``.//yNdhdNmMhossssmdNNdhyyyydhmmdhhdMNmMNNNNNNNh//:``  `   `   ``                                             `.//:-:.                 
 // ```..``.:+o/.``/symhhhdhhssssoomdmd++++++NdNNhyyNMmyNNNNNNNNNo//-```````    .`` `                                       `./+o+/+/.           ```    
 // `.....```:oo:.`-shdhyso/:+ossssyyysyysso+sydyyyhNmy/sNNNmNNNNd+/:-```````  `...```--.                              ``-:/++++oo/.         ``.-:::    
 // `.-.``.```:oo:.:yhhhssooso/-.--::::::/+/://ssssdMNmsyMNNmNNNNNs+/:-`````` ``.--.``./-` ` `-.```                  `:++/:---:/+/` ``  ````-/++++:`    
 // `.--.``..`./ss/ohdyyosss/-............os+/+oossmMMNdhmMNNNNNNNd/:/:.````` `..--.```-. `.` .:-..``.::            -oo+/:::::----.-.--:::::/+o+-`      
 // .`.-:-.``...:syhmdhyyo+//++/:-.....-.-:o+++soshhNMMmyyNMNNNNNNNs::::.```````.::...``` `.` `.:/.``.+-       `.-:/so++//+///+/////-../++++++/------:  
 //```..-:/-.````.+dNdhhhs//oyyso:..-s+sss+/o+oooyhsdNMNddmNNNNNNMNm:----``   ``-::--.` ``.-````-+-`.:/` `    `.--::/o+//://////////:-.-/:--::/++++//:  
 //`````.-::/:.````+Nmhhdss//oyhh-:::yyhyy++syssyydhdNMNhsyNNNNNNNNNo:-...``  .-:/:--.`  `..```./:``-:.     ``.///o//+/:-----:://///:..:+//://:..``     
 // .`````...-..````-yddd+h+/+ohy+yyyshhyooyshhhyydyymMNoohNNNNNNNNNh:-.`..`` -:::-:/:.`  ``.``::. `-/. `   ``/+/+soso+/:-----:::::--..-:----.```       
 // .`..-:/+++:-.--..hmmhyohysydo:sy/odssyyyyddhhhy+ymmdyhdmNmNMMMNNd-:-.....`::..-///:-``  ``.--``.:/.``````-ooooyssssso++/////::::::////:://:::-      
 // `...-:::-------.`hmm+dhssmmdhsssoyddddyshhdddddydhhhso/yNmMNMMNMN``.--.-/:----:+++++/-.` ``.. `-/-..`````/++//://+ossoooooooo+oo++ooo+///////.      
 // `......--.-:-``.-hmNsosdyhmmsysyyhmmhssdhsdddddss+so///dNNNNMMNNN`   `.-:/:://++++++++++:-````-o:....````         `-----:::::::--..`                
 // ````.-.`.:-```.::yNNdsyydmmmmhhyyhmmmddyssmmmd+-:+mmhosmNNNNMMMMN`       ````..----:://+++oo+oss-```````                                            
 // `..`````..````.::oNNNNmhmNNNNmssdmmmmhy/++sssmh+shNhsyhmNmNNNMMMN                   ``````.--oo:-.`                                                 
 //  `.````.-``````:+omNMNMMMMNNNNdhmNNNmms+++osoho///+oymNmmmNNMNNNN                            `.--.`                                                 
 //   `...``.`-```.:hysshmmNNmNNNMMMMMmyo+syyyso+++ossyyshmmmNNNNNNNy                                                                                   
 //    `--::.`-:``.+hyysoosyyyhhyyhddyssyyysssssssssssyhhmNNNNNNNNNN+                                                                                   
 //        `.`.--`.ohhyssssssyyyyyyyhhhhysossssso/--`/dNNNNNNNNMNNNN.                                                                                   
 //         `:..-.-.+sssssssssosyysysyso+/oo+:.`````..-yNNNNNNNNNMNh                                                                                    
 //         dNmho/.-`-++oooo++:oNMNNy+-.```..`````.-.:oymNNNNNNNNm+`                                                                   `.:--`           
 //        .NMMMNNms:..-:::-:+dMMMNNNddhys+:///+ooydmNNNNNNNNNNNh.                                                                       .-.            
 //        -dNMMMMMNNdhyhhhmNMMMNNNNNNNmddddmmddmddddmNNNNmmNmh/`                                                                  ``   `//+:.     ` `  
 //          -yNMMMMMMMMMMMMMMNNNNNNNNNNNmmddddmmmmmmmdhhmmmy/`                                                                     `  `.:--.:`   `     
#endregion

    // 10:25 - 26.6.2015

    public static class ByteHelper
    {
        /*
         * TODO: find memory leaks. Not sure if pointer operations cause it.
         */
        #region Static caches
        // Temp variables used with functions.
        private static readonly int[] aIntCache = new int[1];
        private static readonly int[] bIntCache = new int[1];
        #endregion

        private static readonly byte[] HWORDONE = new byte[1] { 1 };
        private static readonly byte[] WORDONE  = new byte[2] { 1, 0 };
        private static readonly byte[] LWORDONE = new byte[4] { 1, 0, 0, 0 };
        private static readonly byte[] DWORDONE = new byte[8] { 1, 0, 0, 0, 0, 0, 0, 0 };

        private static readonly byte[][] WORDONES = new byte[][]
        {
            HWORDONE,
            WORDONE,
            LWORDONE,
            DWORDONE
        };

        /// <summary>
        /// Converts given bytes to integer. Currently supports
        /// max 4-bytes.
        /// </summary>
        /// <param name="bytes">bytes to convert</param>
        /// <returns>converted value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static int ToInt(params byte[] bytes)
        {
            aIntCache[0] = 0;

            fixed (int* resultPtr = aIntCache)
            {
                byte* resultValPtr = (byte*)resultPtr;

                for (int i = 0; i < bytes.Length; i++) resultValPtr[i] = bytes[i];
            }

            return aIntCache[0];
        }

        /// <summary>
        /// Converts given value to bytes. Currently supports
        /// max 4-bytes.
        /// </summary>
        /// <param name="value">value to convert</param>
        /// <param name="bytes">bytes to convert</param>
        /// <returns>converted bytes</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void ToBytes(int value, byte[] bytes)
        {
            fixed (byte* resultPtr = bytes)
            {
                *(int*)resultPtr = value;
            }
        }

        /// <summary>
        /// Adds two variables presented as bytes together 
        /// and returns the result to the caller. This function supports
        /// max of 4-bytes per variable currently.
        /// </summary>
        /// <param name="lhs">left hand side bytes</param>
        /// <param name="rhs">right hand side bytes</param>
        /// <returns>lhs + rhs</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static int AddInt(byte[] lhs, byte[] rhs)
        {
            aIntCache[0] = 0;
            bIntCache[0] = 0;

            int result = 0;

            fixed (int* lhsResultPtr = aIntCache)
            fixed (int* rhsResultPtr = bIntCache)
            {
                byte* lhsValPtr = (byte*)lhsResultPtr;
                byte* rhsValPtr = (byte*)rhsResultPtr;

                for (int i = 0; i < lhs.Length; i++) lhsValPtr[i] = lhs[i];
                for (int i = 0; i < rhs.Length; i++) rhsValPtr[i] = rhs[i];

                result = aIntCache[0] + bIntCache[0];
            }

            return result;
        }

        /// <summary>
        /// Adds two variables presented as bytes together 
        /// and returns the result to the caller. This function supports
        /// max of 4-bytes per variable currently.
        /// </summary>
        /// <param name="lhs">left hand side bytes</param>
        /// <param name="rhs">right hand side bytes</param>
        /// <returns>lhs + rhs</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void AddBytes(byte[] lhs, byte[] rhs, byte[] bytes)
        {
            aIntCache[0] = 0;
            bIntCache[0] = 0;

            fixed (byte* resultPtr = bytes)
            {
                fixed (int* lhsPtr = aIntCache)
                fixed (int* rhsPtr = bIntCache)
                {
                    byte* lhsValPtr = (byte*)lhsPtr;
                    byte* rhsValPtr = (byte*)rhsPtr;

                    for (int i = 0; i < lhs.Length; i++) lhsValPtr[i] = lhs[i];
                    for (int i = 0; i < rhs.Length; i++) rhsValPtr[i] = rhs[i];

                    int value = aIntCache[0] + bIntCache[0];

                    *(int*)resultPtr = value;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SubtractInt(byte[] lhs, byte[] rhs)
        {
            ByteHelper.Negate(rhs);

            return ByteHelper.AddInt(lhs, rhs);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SubtractBytes(byte[] lhs, byte[] rhs, byte[] bytes)
        {
            ByteHelper.Negate(rhs);

            ByteHelper.AddBytes(lhs, rhs, bytes);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Negate(byte[] bytes)
        {
            for (int i = 0; i < bytes.Length; i++) bytes[i] = (byte)~bytes[i];

            ByteHelper.AddBytes(bytes, WORDONES[bytes.Length - 1], bytes);
        }

        public static string ToBinaryString(byte[] bytes)
        {
            string result = string.Empty;

            for (int i = bytes.Length - 1; i >= 0; i--)
            {
                result += Convert.ToString(bytes[i], 2).PadLeft(8, '0') + " ";
			}

            return result;
        }
    }
}
