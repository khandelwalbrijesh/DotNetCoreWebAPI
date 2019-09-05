using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SampleDotNetCoreApplication.Models;
using SampleDotNetCoreApplication.View;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Collections.Generic;

namespace SampleDotNetCoreApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    internal class FamilyController : ControllerBase
    {
        private readonly FamilyContext _context;

        public FamilyController(FamilyContext context)
        {
            // this means we initialised the database here.
            this._context = context;
            if(_context.Families.Count() == 0)
            {
                Family defaultFamily = new Family();
                defaultFamily.HeadOfFamily = "Krishna";
                defaultFamily.LadyOfFamily = "Sunita";
                defaultFamily.FamilyName = "Khandelwal";
                defaultFamily.NumberOfFamilyMembers = 4;
                defaultFamily.FirstSonOfFamily = "Brijesh";
                defaultFamily.FirstDaughterOFFamily = "Ritu";
                _context.Families.Add(defaultFamily);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(Family), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        internal IActionResult GetFamilies(string familyId)
        {
            List<Family> familyList = new List<Family>();
            foreach(var family in _context.Families)
            {
                familyList.Add(family);
            }

            if (familyList.Count == 0)
            {
                return new NotFoundObjectResult(new Error(StatusCodes.Status404NotFound, "No Family is not found."));
            }

            return Ok(familyList);
        }


        [HttpGet("FamilyId/{familyId}")]
        [ProducesResponseType(typeof(Family), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        internal async Task<IActionResult> GetFamilyAsync(string familyId)
        {
            var family = await _context.Families.FindAsync(familyId);

            if(family == null)
            {
                return new NotFoundObjectResult(new Error(StatusCodes.Status404NotFound, "Given Family is not found."));
            }

            return Ok(family);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Family), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        internal async Task<IActionResult> AddFamilyAsync( [FromBody] FamilyView family)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            Family familyModel = new Family(family);
            await _context.Families.AddAsync(familyModel);
            string guid = familyModel.FamilyId;
            return CreatedAtAction(nameof(familyModel), new { id = familyModel.FamilyId}, familyModel);
        }
    }
}
