﻿using Microsoft.Win32;
using ProyectEF.Entities;
using ProyectEF.Logic;
using ProyectEF.MVC.Models;
using ProyectEF.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectEF.MVC.Controllers
{
    public class ShippersController : Controller
    {

        ShippersLogic logic = new ShippersLogic();
        // GET: Shippers
        public ActionResult Index()
        {   
            ShippersView auxShippersView = new ShippersView();
            List<Shippers> shippers = logic.GetAll();
            List<ShippersView> shippersView = shippers.Select(s=>new ShippersView{
                Id=s.ShipperID.ToString(),
                CompanyName=s.CompanyName,
            }).ToList();
            ViewBag.Shippers = shippersView;
            return View(auxShippersView);
        }

        [HttpPost]
        public ActionResult Insert(ShippersView shippersView)
        {
            try
            {
                Shippers shipperEntity = new Shippers { CompanyName = shippersView.CompanyName };
                if(shipperEntity.CompanyName != null)
                {
                    foreach (char item in shippersView.CompanyName)
                    {
                        if((item >= 33 && item <= 64) || (item >= 91 && item <=96) || (item >= 123 && item <= 255))
                        {
                            throw new FormatException();
                        }
                    }
                }                
                logic.Add(shipperEntity);
                return RedirectToAction("Index");
            }
            catch(FormatException)
            {
                return RedirectToAction("ViewFormatException", "Error");
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException)
            {
                return RedirectToAction("ViewDbEntityValidation", "Error");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                logic.Delete(id);
                return RedirectToAction("Index");
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException)
            {
                return RedirectToAction("ViewDbUpdateException", "Error");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }            
        }

        [HttpPost]
        public ActionResult Update(ShippersView shippersView)
        {
            try
            {
                Shippers shipperEntity = new Shippers {
                    CompanyName = shippersView.CompanyName,
                    ShipperID = Convert.ToInt32(shippersView.Id)
                };
                if (shipperEntity.CompanyName != null && shippersView.Id != null)
                {
                    foreach (char item in shippersView.CompanyName)
                    {
                        if ((item >= 33 && item <= 64) || (item >= 91 && item <= 96) || (item >= 123 && item <= 255))
                        {
                            throw new FormatException();
                        }
                    }
                    foreach (char item in shippersView.Id)
                    {
                        if (item <= 48 && item >= 58)
                        {
                            throw new FormatException();
                        }
                    }
                }
                else
                {
                    throw new NullStringException();
                }
                logic.Update(shipperEntity);
                return RedirectToAction("Index");
            }
            catch (NullStringException)
            {
                return RedirectToAction("ViewDbEntityValidation", "Error");
            }
            catch (FormatException)
            {
                return RedirectToAction("ViewFormatException", "Error");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }
        }
    }
}