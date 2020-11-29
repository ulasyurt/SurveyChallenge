using surveychallenge.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace surveychallenge.Controllers
{
    public class HomeController : Controller
    {
        SurveyChallengeEntities db = new SurveyChallengeEntities();

        public ActionResult Index()
        {
            return View();
        }
        [Route("~/about-us")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [Route("~/our-surveys")]
        public ActionResult Survey()
        {
            try
            {
                return View(db.Survey.Where(x => x.SurveyStatus == 1).ToList());
            }
            catch (Exception ex)
            {
                return View("Index");
            }
        }
        [Route("~/our-surveys/{SurveyId}")]
        public ActionResult SurveyDetail(int SurveyId)
        {
            try
            {
                if (db.Survey.FirstOrDefault(x => x.SurveyId == SurveyId) != null)
                {
                    return View(db.Survey.FirstOrDefault(x => x.SurveyId == SurveyId));
                }
                else
                {
                    return RedirectToAction("Survey");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Survey");
            }
        }
        [Route("~/our-closed-surveys")]
        public ActionResult ClosedSurvey()
        {
            try
            {
                return View(db.Survey.Where(x => x.SurveyStatus == 0).ToList());
            }
            catch (Exception ex)
            {
                return View("Index");
            }
        }
        [Route("~/our-closed-surveys/{SurveyId}")]
        public ActionResult SurveyClosedDetail(int SurveyId)
        {
            try
            {
                if (db.Survey.FirstOrDefault(x => x.SurveyId == SurveyId) != null)
                {
                    return View(db.Survey.FirstOrDefault(x => x.SurveyId == SurveyId));
                }
                else
                {
                    return RedirectToAction("Survey");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Survey");
            }
        }
        [HttpPost]
        public ActionResult Submit(Submit submit)
        {
            try
            {
                if (db.Survey.FirstOrDefault(x => x.SurveyId == submit.SurveyId) != null && db.Survey.FirstOrDefault(x => x.SurveyId == submit.SurveyId).SurveyStatus!=0)
                {
                    db.Survey.FirstOrDefault(x => x.SurveyId == submit.SurveyId).SubmitCount = db.Survey.FirstOrDefault(x => x.SurveyId == submit.SurveyId).SubmitCount + 1;
                    for (int i = 0; i < submit.SelectedOptions.Count; i++)
                    {
                        int temp = submit.SelectedOptions[i];
                        if (db.Option.FirstOrDefault(x => x.OptionId == temp) != null)
                        {
                            db.Option.FirstOrDefault(x => x.OptionId == temp).SubmitCount = db.Option.FirstOrDefault(x => x.OptionId == temp).SubmitCount + 1;
                        }
                    }
                    db.SaveChanges();
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}