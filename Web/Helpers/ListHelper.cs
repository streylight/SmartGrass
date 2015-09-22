using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Core;
using Core.Domains;
using Service.Interfaces;

namespace Web.Helpers {
    /// <summary>
    /// The ListHelper class used for HTML dropdowns and select2s
    /// </summary>
    public static class ListHelper {

        /// <summary>
        /// Returns all IrrigationValves for a given unit
        /// </summary>
        /// <param name="unitId"></param>
        /// <returns></returns>
        public static List<IrrigationValve> GetListOfIrrigationValves( int unitId ) {
            var irrigationValveService = new IrrigationValveService();
            return irrigationValveService.GetAllIrrigationValves().Take( 3 ).ToList(); // take 3 for prototype demo purposes only
        }

        /// <summary>
        /// Returns the last 7 days in DateTime format
        /// </summary>
        /// <param name="now"></param>
        /// <returns></returns>
        public static List<DateTime> GetPreviousWeekDates( DateTime now ) {
            var startingDate = now.AddDays( -7 );
            var datesList = new List<DateTime> { startingDate };
            for ( var i = 1; i < 7; i++ ) {
                datesList.Add( startingDate.AddDays( i ) );
            }
            return datesList;
        }

        /// <summary>
        /// Returns a key-value pair for the Role enums
        /// </summary>
        /// <returns></returns>
        public static Dictionary<Role, string> GetListRoles() {
            return Enum.GetValues( typeof( Role ) ).Cast<object>().ToDictionary( at => ( Role )at, at => GetEnumDescription( ( Role ) at ) );
        }

        /// <summary>
        /// Returns the string name for a given enum
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string GetEnumDescription( Enum value ) {
            var fi = value.GetType().GetField( value.ToString() );
            var attributes = ( DescriptionAttribute[] )fi.GetCustomAttributes( typeof( DescriptionAttribute ), false );

            if ( attributes != null && attributes.Length > 0 ) {
                return attributes[0].Description;
            }
            return value.ToString();
        }
    } // class
} // namespace